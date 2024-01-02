using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

public class ColorPalette
{
    public readonly MemoryPixel Background = MemoryPixel.FromByte(42);
    public readonly MemoryPixel Hotbar = MemoryPixel.FromByte(53);
    public readonly MemoryPixel SelectedCapHotbar = MemoryPixel.FromGDIColor(Color.Magenta);
}