using NoNameOS.Kernel.DisplayManagement;
using NoNameOS.Win;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TuringAPI;

public class OS
{
    public OS(OSArguments args, params App[] apps)
    {
        InitArgs = args;

        InitDisplay(); LogInit("Display");
        InitStartResources(); LogInit("Start Resources");
        ShowStartLogo(); LogInfo("Shown Start-logo");
        InitHotbar(); LogInit("Hotbar");
        InitResources(); LogInit("Resources");
        InitIAPI(); LogInit("Iterception API");
        HideLogo(); LogInfo("Hidden Start-logo");
        InitApps(apps); LogInit("Inited Apps");
        DrawHotbar(); LogInit("Drawn Hotbar");
        DrawInterface(); LogInfo("Drawn Interface");

        StartUpdater();
    }

    const Key ActiveMouseKey = Key.NumLock;
    const int CursorSize = 6;
    const int CursorSizeD2 = CursorSize / 2;

    public readonly OSArguments InitArgs;

    public ColorPalette Palette = new();

    public IterceptionManager IAPI;
    public MemoryBitmap Canvas;

    public WinDisplay? WinDisplay;
    public Display PhysDisplay;

    public VirtualDisplay Display;
    public VirtualDisplay WorkspaceDisplay;

    public Hotbar Hotbar;

    double RealMouseX;
    double RealMouseY;
    public double MouseX;
    public double MouseY;
    public double PrevMouseX;
    public double PrevMouseY;
    double MouseMoveX;
    double MouseMoveY;

    public int AppMouseX => (int)MouseX - Hotbar.Width;
    public int AppMouseY => (int)MouseY;

    public App CurrentApp;
    public List<App> Apps = new();

    MemoryBitmap StartLogoBitmap;
    bool[,] CursorGrayMap;
    MemoryBitmap CursorBitmap;

    public bool IsActiveMouse => (Interop.GetKeyState(Keys.NumLock) & (1 << 0)) == 0;

    #region Init
    void InitDisplay() 
    {
        Canvas = new(TuringAPI.Display.WIDTH, TuringAPI.Display.HEIGHT);
        var displays = new List<IDisplay>();
        #region Physical Display
        PhysDisplay = new();

        while (!PhysDisplay.Open())
        {
            Console.Beep();
            Thread.Sleep(1000);
        }

        PhysDisplay.SetOrientation(Orientations.ReverseLandscape);
        PhysDisplay.SetBrightness(.2);
        displays.Add(PhysDisplay);
        #endregion

        #region Virtual Windows Display
        if (InitArgs.ShowVirtualWindowsDisplay)
        {
            WinDisplay = new();
            displays.Add(WinDisplay);
        }
        #endregion
                
        Display = new VirtualDisplay(Canvas, displays).Init();

        WorkspaceDisplay = new VirtualDisplay(Canvas, Display)
        {
            Left = Hotbar.Width,
        }.Init();

        if (InitArgs.ClearDisplay)
            Display.ClearByBlack();
    }

    void InitStartResources()
    {
        #region StartLogo
        if (InitArgs.ShowLogo)
        {
            using var bitmap = new Bitmap(PhysDisplay.Width, PhysDisplay.Height);
            using var g = Graphics.FromImage(bitmap);

            var title = "NoName OS";
            var titleFont = new Font("Microsoft Sans Serif", 32, FontStyle.Italic);

            var lineFont = new Font("Microsoft Sans Serif", 16);

            var brush = new SolidBrush(Color.FromArgb(150, 150, 150)); //Gray - 128

            var size = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, brush, new PointF((PhysDisplay.Width - size.Width) / 2, 100));

            float startY = 0;
            foreach (var line in new string[] {
            "made by yotic",
            "date: " + DateTime.Now,
        })
            {
                g.DrawString(line, lineFont, brush, 0, startY);
                startY += g.MeasureString(line, lineFont).Height;
            }

            StartLogoBitmap = MemoryBitmap.FromGDIBitmap(bitmap);
        }
        #endregion
    }

    void ShowStartLogo()
    {
        if (InitArgs.ShowLogo)
            Display.FillBitmapByParts(StartLogoBitmap);
    }

    void InitHotbar()
    {
        Hotbar = new Hotbar(this);
    }

    void InitResources()
    {
        #region Cursor
        CursorBitmap = new MemoryBitmap(CursorSize, CursorSize);
        for (int y = 0; y < CursorSize; y++)
            for (int x = 0; x < CursorSize; x++)
                CursorBitmap[x, y] = MemoryPixel.Black;

        for (int y = 0; y < CursorSize; y++)
            for (int x = 0; x < CursorSize; x++)
                CursorBitmap[x, y] = MemoryPixel.White;

        CursorBitmap[0, 0] = MemoryPixel.Black;
        CursorBitmap[CursorSize - 1, 0] = MemoryPixel.Black;
        CursorBitmap[CursorSize - 1, CursorSize - 1] = MemoryPixel.Black;
        CursorBitmap[0, CursorSize - 1] = MemoryPixel.Black;

        CursorGrayMap = new bool[CursorSize, CursorSize];
        for (int y = 0; y < CursorSize; y++)
            for (int x = 0; x < CursorSize; x++)
                CursorGrayMap[x, y] = CursorBitmap[x, y].Value != 0;
        #endregion
    }

    void InitIAPI()
    {
        IAPI = new();
        IAPI.Start();

        IAPI.OnMouseMove += OnMouseMove;
        IAPI.OnKeyDown += OnKeyDown;
        IAPI.OnKeyUp += OnKeyUp;
    }

    void HideLogo()
    {
        using var bitmap = new MemoryBitmap(PhysDisplay.Width, PhysDisplay.Height);
        bitmap.Clear(Palette.Background.Value);

        Display.DrawBitmap(bitmap);
    }

    void InitApps(App[] apps)
    {
        Apps.AddRange(apps);

        foreach (var app in apps)
        {
            app.SetOS(this);
            app.Init();
        }
    }

    void DrawHotbar()
    {
        Hotbar.InitDraw();
    }

    void DrawInterface()
    {
        Hotbar.DrawBackgroud();

        for (int i = 0; i < Apps.Count; i++)
        {
            App app = Apps[i];
            Display.DrawBitmap(app.Icon, 0, i * Hotbar.IconSize);
        }
    }

    void StartUpdater()
    {
        while (true)
        {
            Update();
            Thread.Sleep(5);
        }
    }
    #endregion

    void Log(string line) => Console.WriteLine(line);
    void LogInit(string line) => Console.WriteLine("[init] Inited " + line);
    void LogInfo(string line) => Console.WriteLine("[info] " + line);

    void Update()
    {
        #region Render Cursor
        PrevMouseX = MouseX;
        PrevMouseY = MouseY;

        MouseX = Math.Clamp(RealMouseX, 0, PhysDisplay.Width);
        MouseY = Math.Clamp(RealMouseY, 0, PhysDisplay.Height);

        if (MouseX == PrevMouseX && MouseY == PrevMouseY)
            return;

        RealMouseX = MouseX;
        RealMouseY = MouseY;

        MouseMoveX = MouseX - PrevMouseX;
        MouseMoveY = MouseY - PrevMouseY;

        for (int py = 0; py < CursorSize; py++)
            for (int px = 0; px < CursorSize; px++)
                if (!CursorGrayMap[px, py])
                    CursorBitmap[px, py] = Display.Canvas[(int)MouseX + px, (int)MouseY + py];

        Display.UpdateBufferedRegion((int)(PrevMouseX - CursorSizeD2), (int)(PrevMouseY - CursorSizeD2), CursorSize, CursorSize);
        Display.DrawBitmapWithoutBuffering(CursorBitmap, (int)(MouseX - CursorSizeD2), (int)(MouseY - CursorSizeD2));
        #endregion

        if (CurrentApp != null)
        {
            CurrentApp.Update();
        }
    }

    public void OpenApp(App app)
    {
        CurrentApp = app;
        app.Open();
    }

    public void OpenApp(int index) => OpenApp(Apps[index]);

    public void CloseApp()
    {
        if (CurrentApp != null)
        {
            CurrentApp.Close();
            CurrentApp = null;
        }
    }

    #region Iterception Events
    bool OnMouseMove(int x, int y)
    {
        if (IsActiveMouse)
        {
            RealMouseX += x / 2d;
            RealMouseY += y / 2d;

            if (CurrentApp != null)
            {
                CurrentApp.OnMouseMove((int)RealMouseX, (int)RealMouseY);
            }

            return true;
        }
        else
        {
            if (CurrentApp != null)
            CurrentApp.OnMouseMove  (x, y);
        }

        return false;
    }

    bool OnKeyDown(Key key, bool repeat)
    {
        if (repeat)
            return false;

        if (IsActiveMouse)
        {
            if (MouseX <= Hotbar.Width)
            {
                int index = (int)(MouseY / Hotbar.IconSize);
                Hotbar.Select(index);
            }
            else
            {
                if (CurrentApp != null)
                {
                    if (key == Key.MouseLeft)
                    {
                        CurrentApp.OnMouseClick(true);
                        CurrentApp.OnLeftMouseClick();
                    }
                    else if (key == Key.MouseRight)
                    {
                        CurrentApp.OnMouseClick(false);
                        CurrentApp.OnRightMouseClick();
                    }

                    CurrentApp.OnKeyDown(key);
                }
            }
        }
        else
        {
            if (CurrentApp != null)
            {
                if (key == Key.MouseLeft)
                {
                    CurrentApp.OnExternalMouseClick(true);
                    CurrentApp.OnExternalLeftMouseClick();
                }
                else if (key == Key.MouseRight)
                {
                    CurrentApp.OnExternalMouseClick(false);
                    CurrentApp.OnExternalRightMouseClick();
                }

                CurrentApp.OnExternalKeyDown(key);
            }
        }

        return false;
    }

    bool OnKeyUp(Key key)
    {
        if (IsActiveMouse)
        {
            if (key != ActiveMouseKey)
            {
                return true;
            }

            if (CurrentApp != null)
            {
                CurrentApp.OnKeyUp(key);
            }
        }
        else
        {
            if (CurrentApp != null)
                CurrentApp.OnExternalKeyUp(key);
        }

        return false;
    }
    #endregion

    public class OSArguments
    {
        public bool ClearDisplay;
        public bool ShowLogo;
        public bool ShowVirtualWindowsDisplay;
    }
}