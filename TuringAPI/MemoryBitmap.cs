using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TuringAPI;
public unsafe class MemoryBitmap : IDisposable
{
    public MemoryBitmap(int width, int height, void* ptr)
    {
        Width = width;
        Height = height;

        ByteData = (byte*)ptr;
        PixelData = (MemoryPixel*)ptr;
    }

    public MemoryBitmap(int width, int height) : this(width, height, (void*)Marshal.AllocCoTaskMem(width * height * 3)) { }

    public readonly int Width, Height;
    public byte* ByteData;
    public MemoryPixel* PixelData;

    public MemoryBitmap FillRect(int x, int y, int width, int height, MemoryPixel pixel)
    {
        int ex = x + width;
        int ey = y + height;

        for (int py = y; py < ey; py++)
            for (int px = x; px < ex; px++)
                this[px, py] = pixel;

        return this;
    }

    public MemoryBitmap Fill(MemoryPixel pixel) => Clear(pixel.Value); // FillRect(0, 0, Width, Height, pixel);

    public MemoryBitmap Clear(short val = 0)
    {
        short* start = (short*)ByteData;
        short* end = start + Width * Height;
        for (; start < end; start++)
            *start = val;

        return this;
    }

    public MemoryBitmap Clear(MemoryPixel pixel) => Clear(pixel.Value);

    public void SetPointer(void* ptr)
    {
        ByteData = (byte*)ptr;
        PixelData = (MemoryPixel*)ptr;
    }

    public byte[] ToArray()
    {
        var bytes = new byte[Width * Height * sizeof(short)];
        fixed (byte* bytesPtr = bytes)
            Buffer.MemoryCopy(ByteData, bytesPtr, bytes.Length, bytes.Length);
        return bytes;
    }

    public byte[] CopyRegionToBytes(int x, int y, int width, int height)
    {
        var bytes = new byte[width * height * sizeof(short)];
        fixed (byte* bytesPrt = bytes)
        {
            var ptr = (MemoryPixel*)bytesPrt;
            for (int py = 0; py < height; py++)
                for (int px = 0; px < width; px++)
                    ptr[py * width + px] = this[x + px, y + py];
        }

        return bytes;
    }

    public MemoryPixel this[int x, int y]
    {
        get => PixelData[y * Width + x];
        set => PixelData[y * Width + x] = value;
    }

    public static MemoryBitmap FromDataFile(string path, int width, int height)
    {
        var bytes = File.ReadAllBytes(path);
        var bitmap = new MemoryBitmap(width, height);
        fixed (byte* bytesPtr = bytes)
            Buffer.MemoryCopy(bytesPtr, bitmap.ByteData, bytes.Length, bytes.Length);
        return bitmap;
    }

    public static MemoryBitmap FromGDIBitmap(string path)
    {
        using var gdiBitmap = (Bitmap)Bitmap.FromFile(path);
        return FromGDIBitmap(gdiBitmap);
    }

    public static MemoryBitmap FromGDIBitmap(Bitmap gdiBitmap)
    {
        var bitmap = new MemoryBitmap(gdiBitmap.Width, gdiBitmap.Height);
        for (int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
                bitmap[x, y] = MemoryPixel.FromGDIColor(gdiBitmap.GetPixel(x, y));
        return bitmap;
    }

    //reserve
    public static MemoryBitmap FromGDIBitmapFast(Bitmap gdiBitmap)
    {
        var data = gdiBitmap.LockBits(new Rectangle(0, 0, gdiBitmap.Width, gdiBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        var bitmap = new MemoryBitmap(gdiBitmap.Width, gdiBitmap.Height);
        for (int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
                bitmap[x, y] = MemoryPixel.FromGDIColor(gdiBitmap.GetPixel(x, y));

        gdiBitmap.UnlockBits(data);

        return bitmap;
    }

    public static MemoryBitmap FromMemoryHBitmap(MemoryHBitmap bitmap, int width, int height) => new(width, height, bitmap.Pixels);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Marshal.FreeCoTaskMem((nint)ByteData);
    }
}