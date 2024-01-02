using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Interop;

public unsafe class MemoryHBitmap
{
    public MemoryHBitmap(RGB* pixels, nint hBitmap)
    {
        Pixels = pixels;
        HBitmap = hBitmap;
    }

    public RGB* Pixels;
    public nint HBitmap;

    public Bitmap GetGDIBitmap() => Bitmap.FromHbitmap(HBitmap);
    public Bitmap GetGDIBitmap2(int width, int height)
    {
        Bitmap bmp = new Bitmap(width, height);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                RGB rgb = GetPixel(x, y);
                bmp.SetPixel(x, y, Color.FromArgb(rgb.B, rgb.G, rgb.R));
            }

        return bmp;
    }

    public RGB GetPixel(int x, int y) => Pixels[(HEIGHTM1 - y) * WIDTH + x];
    public BitmapRegion GetRegionBySize(int x, int y, int width, int height) => new(this, x, y, x + width, y + height);
    public BitmapRegion GetRegionByRect(int x, int y, int x2, int y2) => new(this, x, y, x2, y2);
}