using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

namespace NoNameOS.Kernel.DisplayManagement;
public unsafe partial class VirtualDisplay : IDisplay
{
    public const int WIDTH = Display.WIDTH, HEIGHT = Display.HEIGHT;

    public VirtualDisplay(MemoryBitmap canvas, params IDisplay[] displays) : this(canvas, displays.ToList()) { }

    public VirtualDisplay(MemoryBitmap canvas, List<IDisplay> displays)
    {
        Canvas = canvas;
        Displays = displays;
    }

    public MemoryBitmap Canvas;
    public readonly List<IDisplay> Displays;
    public int Left = 0, Top = 0, Right = WIDTH, Bottom = HEIGHT;
    public int Width, Height;

    public VirtualDisplay Init()
    {
        Width = Right - Left;
        Height = Bottom - Top;

        return this;
    }

    void ForEach(Action<IDisplay> action)
    {
        foreach (var display in Displays)
            action(display);
    }

    public override void Draw(int x, int y, int width, int height, byte[] bytes) => ForEach(d => d.Draw(x + Left, y + Top, width, height, bytes));
}