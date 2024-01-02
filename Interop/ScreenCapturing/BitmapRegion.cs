using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Interop;

public unsafe class BitmapRegion
{
    public BitmapRegion(MemoryHBitmap bitmap, int x, int y, int x2, int y2)
    {
        Bitmap = bitmap;
        X = x;
        Y = y;
        X2 = x2;
        Y2 = y2;
    }

    public MemoryHBitmap Bitmap;
    public int X, Y, X2, Y2;
    public int Width => X2 - X;
    public int Height => Y2 - Y;

    public RGB GetPixel(int x, int y) => Bitmap.Pixels[(HEIGHTM1 - (Y + y)) * WIDTH + (X + x)];

    public Bitmap GetDBitmap()
    {
        Bitmap bmp = new Bitmap(Width, Height);
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                RGB pixel = GetPixel(x, y);
                bmp.SetPixel(x, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));
            }
        return bmp;
    }

    public void Save(string path)
    {
        using Bitmap bmp = GetDBitmap();
        bmp.Save(path, ImageFormat.Bmp);
    }
}