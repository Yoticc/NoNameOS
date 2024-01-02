using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public unsafe class CapturedScreen
{
    public CapturedScreen(MemoryHBitmap bitmap)
    {
        Bitmap = bitmap;
    }

    public MemoryHBitmap Bitmap;
    public int Width = 1920;
    public int Height = 1080;
}