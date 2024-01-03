using NoNameOS.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;
using static Key;
using static System.Windows.Forms.LinkLabel;

namespace NoNameOS.Apps.Terminal;
public class TerminalApp() : App("Terminal")
{
    #region Init
    public override void Init()
    {
        inputLineBitmap = new Bitmap(OS.WorkspaceDisplay.Width, 24);
        inputLineBitmapGraphics = Graphics.FromImage(inputLineBitmap);

        linesBitmap = new Bitmap(OS.WorkspaceDisplay.Width, OS.WorkspaceDisplay.Height - 24);
        linesBitmapGraphics = Graphics.FromImage(linesBitmap);
    }

    public override MemoryBitmap GetIcon()
    {
        using var bitmap = NewGDIBitmapForIcon;
        using var g = Graphics.FromImage(bitmap);

        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), IconGDIRect);

        var brush = new SolidBrush(Color.Silver);
        g.DrawString(">", new Font("Consolas", 20), brush, -2, -1);
        g.DrawString("_", new Font("MS UI Gothic", 20), brush, 15, -1);

        return MemoryBitmap.FromGDIBitmap(bitmap);
    }
    #endregion

    public Font LineFont = new("Consolas", 14);
    public SolidBrush InputLineBrush = new(Color.Green);
    public SolidBrush LinesBrush = new(Color.DarkGreen);

    public List<string> Lines = new();
    public string CurrentInputLine = "";
    public string? SelectedTerminal;

    string WrapToUserLine(string line) => $"${OS.UserName}{(SelectedTerminal != null ? $"-{SelectedTerminal[0]}" : "")}> {line}";

    Bitmap inputLineBitmap;
    Graphics inputLineBitmapGraphics;

    Bitmap linesBitmap;
    Graphics linesBitmapGraphics;

    public override void Open()
    {
        DrawInputLine();
    }

    List<Key> numKeys = [D0, D1, D2, D3, D4, D5, D6, D7, D8, D9];
    public override void OnKeyDown(Key key)
    {
        if (key == Enter)
        {
            HandleCommand();
        }

        var keyString = key.ToString();
        if (keyString.Length == 1)
        {
            var isCapital = OS.IsActiveCapsLock ? !OS.IsActiveShift : OS.IsActiveShift;

            CurrentInputLine += isCapital ? keyString : keyString.ToLower();
        }
        else if (key == Backspace)
        {
            if (CurrentInputLine.Length == 0)
                goto RET;

            if (OS.IsActiveControl)
            {
                var index = CurrentInputLine.FindLastCharIndex(' ');
                if (index == -1)
                    CurrentInputLine = "";
                else CurrentInputLine = CurrentInputLine.Substring(0, index + 1);
            }
            else
            {
                CurrentInputLine = CurrentInputLine.Substring(0, CurrentInputLine.Length - 1);
            }
        }
        else if (numKeys.Contains(key))
        {
            CurrentInputLine += numKeys.IndexOf(key).ToString();
        }
        else if (key == Space)
        {
            CurrentInputLine += ' ';
        }
        else
        {
            return;
        }

        DrawInputLine();

    RET: { }
    }

    void DrawInputLine()
    {
        var line = WrapToUserLine(CurrentInputLine);

        inputLineBitmapGraphics.FillRectangle(new SolidBrush(OS.Palette.GDIBackground), new(0, 0, inputLineBitmap.Width, inputLineBitmap.Height));
        inputLineBitmapGraphics.DrawString(line, LineFont, InputLineBrush, 0, 0);
        using var inputLineMemoryBitmap = MemoryBitmap.FromGDIBitmap(inputLineBitmap);
        Display.DrawBitmap(inputLineMemoryBitmap, 0, Display.Height - inputLineBitmap.Height);
    }

    void HandleCommand()
    {
        var command = CurrentInputLine;
        CurrentInputLine = "";

        if (command == "cmd")
        {
            SelectedTerminal = "cmd";
        }
        else if (command == "help")
        {

        }
        else
        {
            AddLines(WrapToUserLine(command));
        }

        DrawInputLine();
    }

    void AddLines(params string[] lines)
    {
        Lines.AddRange(lines);
        linesBitmapGraphics.FillRectangle(new SolidBrush(OS.Palette.GDIBackground), new(0, 0, linesBitmap.Width, linesBitmap.Height));
        linesBitmapGraphics.DrawString(string.Join('\n', Lines), LineFont, LinesBrush, 0, 0);
        using var linesMemoryBitmap = MemoryBitmap.FromGDIBitmap(linesBitmap);
        Display.DrawBitmap(linesMemoryBitmap, 0, 0);
    }
}