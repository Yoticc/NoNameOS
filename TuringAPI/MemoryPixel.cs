using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static Interop;

namespace TuringAPI;
[StructLayout(LayoutKind.Sequential)]
public struct MemoryPixel
{
    public MemoryPixel(byte r, byte g, byte b) => Value = (short)(((r >> 3) << 11) | ((g >> 2) << 5) | (b >> 3));

    public short Value;

    public Color ToGDIColor()
    {
        byte r = (byte)(((Value >> 11) & 0x1F) << 3);
        byte g = (byte)(((Value >> 5) & 0x3F) << 2);
        byte b = (byte)((Value & 0x1F) << 3);

        return Color.FromArgb(r, g, b);
    }

    public static MemoryPixel FromGDIColor(Color color) => new(color.R, color.G, color.B);

    public static MemoryPixel Black = new(0, 0, 0);
    public static MemoryPixel White = new(0xFF, 0xFF, 0xFF);
    public static MemoryPixel Red = new(0xFF, 0, 0);
    public static MemoryPixel Blue = new(0, 0xFF, 0);
    public static MemoryPixel Green = new(0, 0, 0xFF);

    public static MemoryPixel FromByte(byte color) => new MemoryPixel(color, color, color);
}