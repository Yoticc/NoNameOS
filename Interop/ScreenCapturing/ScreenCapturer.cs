using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public unsafe class ScreenCapturer
{
    private bool working;
    public void Start() => new Thread(InternalStart).Start();

    public int SleepTime = 50;
    public int X, Y, Width, Height;
    private void InternalStart()
    {
        working = true;
        var hdc = GetWindowDC(0);

        var bitsPerPixel = 24;
        var bytesPerPixel = bitsPerPixel / 8;

        var compatibleDC = CreateCompatibleDC(hdc);

        var bmi = new BITMAPINFO()
        {
            Header = new()
            {
                Size = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER)),
                Width = Width,
                Height = Height,
                Planes = 1,
                BitCount = (ushort)bitsPerPixel,
            }
        };

        RGB* pixels = null;
        var hBitmap = CreateDIBSection(hdc, &bmi, 0, &pixels, nint.Zero, 0);
        var oldBitmap = SelectObject(compatibleDC, hBitmap);

        var mBitmap = new MemoryHBitmap(pixels, hBitmap);
        var screen = new CapturedScreen(mBitmap);
        SetCapture?.Invoke(screen);
        while (working)
        {
            try
            {
                //X = 100; Y = 200; Width = Globals.WIDTH; Height = Globals.HEIGHT;
                //Thread.Sleep(SleepTime);

                if (GetForegroundWindow() != Game.HWnd)
                    continue;

                BitBlt(compatibleDC, 0, 0, Width, Height, hdc, X, Y, CopyPixelOperation.SourceCopy);

                //Console.WriteLine($"a: {X} {Y} {Width} {Height}");
                NewFrame?.Invoke();
            }
            catch (Exception ex)
            {

            }
        }

        SelectObject(compatibleDC, oldBitmap);
        DeleteObject(hBitmap);
        DeleteDC(compatibleDC);
        ReleaseDC(0, hdc);
    }

    public void End() => working = false;

    public delegate void NewFrameHandler();
    public event NewFrameHandler? NewFrame;

    public delegate void SetCaptureHandler(CapturedScreen s);
    public event SetCaptureHandler? SetCapture;
}