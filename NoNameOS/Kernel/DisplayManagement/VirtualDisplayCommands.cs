using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TuringAPI;

namespace NoNameOS.Kernel.DisplayManagement;
public unsafe partial class VirtualDisplay
{
    public void ClearByBlack() => Draw(0, 0, WIDTH, HEIGHT, Display.BlackBitmap);

    public void DrawBitmap(MemoryBitmap bitmap) => DrawBitmap(bitmap, 0, 0);

    public void DrawBitmap(MemoryBitmap bitmap, int x, int y)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int spx = x < 0 ? -x : 0;
        int spy = y < 0 ? -y : 0;
        width -= spx;
        height -= spy;
        x += spx;
        y += spy;

        for (int ry = 0; ry < height; ry++)
            for (int rx = 0; rx < width; rx++)
                Canvas[x + rx, y + ry] = bitmap[rx, ry];

        Draw(x, y, width, height, bitmap);
    }

    public void DrawBitmapWithoutBuffering(MemoryBitmap bitmap, int x, int y)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int spx = x < 0 ? -x : 0;
        int spy = y < 0 ? -y : 0;
        width -= spx;
        height -= spy;
        x += spx;
        y += spy;

        Draw(x, y, width, height, bitmap);
    }

    public void UpdateBufferedRegion(int x, int y, int width, int height)
    {
        int spx = x < 0 ? -x : 0;
        int spy = y < 0 ? -y : 0;
        width -= spx;
        height -= spy;
        x += spx;
        y += spy;

        int index = 0;
        var bytes = new byte[width * height * sizeof(short)];
        fixed (byte* bytesPtr = bytes)
        {
            var pixelPtr = (MemoryPixel*)bytesPtr;
            for (int ry = spx; ry < width; ry++)
                for (int rx = spy; rx < height; rx++)
                    pixelPtr[index++] = Canvas[x + rx, y + ry];
        }

        Draw(x, y, width, height, bytes);
    }

    public void FastFillBitmap(MemoryBitmap bitmap)
    {
        int bitmapWidth = bitmap.Width;
        int bitmapHeight = bitmap.Height;
        int displayWidth = WIDTH;
        int displayHeight = HEIGHT;
        int xStart = 0, yStart = 0, xEnd = displayWidth, yEnd = displayHeight;

        bool broken = false;

        for (int x = 0; x < displayWidth; x++)
        {
            broken = false;
            for (int y = 0; y < displayHeight; y++)
                if (bitmap[x, y].Value != Canvas[x, y].Value)
                {
                    broken = true;
                    break;
                }

            if (broken)
            {
                xStart = x;
                break;
            }
        }

        if (!broken)
            return;

        for (int x = displayWidth; x >= 0; x--)
        {
            broken = false;
            for (int y = 0; y < displayHeight; y++)
                if (bitmap[x, y].Value != Canvas[x, y].Value)
                {
                    broken = true;
                    break;
                }

            if (broken)
            {
                xEnd = x;
                break;
            }
        }

        for (int y = 0; y < displayHeight; y++)
        {
            broken = false;
            for (int x = 0; x < displayWidth; x++)
                if (bitmap[x, y].Value != Canvas[x, y].Value)
                {
                    broken = true;
                    break;
                }

            if (broken)
            {
                yStart = y;
                break;
            }
        }

        if (!broken)
            return;

        for (int y = displayHeight; y >= 0; y--)
        {
            broken = false;
            for (int x = 0; x < displayWidth; x++)
                if (bitmap[x, y].Value != Canvas[x, y].Value)
                {
                    broken = true;
                    break;
                }

            if (broken)
            {
                yEnd = y;
                break;
            }
        }

        for (int y = 0; y < bitmapHeight; y++)
            for (int x = 0; x < bitmapWidth; x++)
                Canvas[x, y] = bitmap[x, y];

        xEnd += 1;
        yEnd += 1;

        int width = xEnd - xStart;
        int height = yEnd - yStart;
        if (width <= 1 || height <= 1)
            return;

        byte[] bytes = bitmap.CopyRegionToBytes(xStart, yStart, width, height);

        Draw(xStart, yStart, width, height, bytes);
    }

    public void FillBitmapByParts(MemoryBitmap bitmap, int splitStrength = 80)
    {
        int bitmapWidth = bitmap.Width;
        int bitmapHeight = bitmap.Height;
        int partWidth = Width / splitStrength;
        int partHeight = Height / splitStrength;

        for (int yp = 0; yp < splitStrength; yp++)
            for (int xp = 0; xp < splitStrength; xp++)
            {
                bool isDiffer = false;
                int sX = xp * partWidth;
                int eX = sX + partWidth;
                int sY = yp * partHeight;
                int eY = sY + partHeight;
                for (int y = sY; y < eY; y++)
                    for (int x = sX; x < eX; x++)
                        if (bitmap[x, y].Value != Canvas[x, y].Value)
                        {
                            isDiffer = true;
                            goto DIF;
                        }

                    DIF:
                if (isDiffer)
                {
                    byte[] bytes = bitmap.CopyRegionToBytes(sX, sY, partWidth, partHeight);

                    Draw(sX, sY, eX - sX, eY - sY, bytes);
                }
            }

        for (int y = 0; y < bitmapHeight; y++)
            for (int x = 0; x < bitmapWidth; x++)
                Canvas[x, y] = bitmap[x, y];
    }

    public void VirtualFillBitmapByParts(MemoryBitmap bitmap, int xStart, int splitStrength = 80)
    {
        try
        {
            int bitmapWidth = bitmap.Width;
            int bitmapHeight = bitmap.Height;
            int partWidth = Width / splitStrength;
            int partHeight = Height / splitStrength;

            int notFullDest = xStart % partWidth;
            bool hasNoFullPart = notFullDest != 0;
            int notFullIndex = xStart / partWidth;



            int startXp = hasNoFullPart ? notFullIndex + 1 : 0;
            for (int yp = 0; yp < splitStrength; yp++)
                for (int xp = startXp; xp < splitStrength; xp++)
                {
                    //Console.WriteLine($"b: {xp} {xStart} {notFullIndex} {notFullDest}");

                    bool isDiffer = false;
                    int sX = xp * partWidth;
                    int eX = sX + partWidth;
                    int sY = yp * partHeight;
                    int eY = sY + partHeight;
                    for (int y = sY; y < eY; y++)
                        for (int x = sX; x < eX; x++)
                            if (bitmap[x, y].Value != Canvas[x, y].Value)
                            {
                                isDiffer = true;
                                goto DIF;
                            }

                        DIF:
                    if (isDiffer)
                    {

                        int realWidth = (sX - xStart + partWidth) > Width ? (Width - (sX - xStart)) : partWidth;
                        //Console.WriteLine($"{sX} {sY} {eX} {eY} {realWidth} {xStart} {sX - xStart}");
                        byte[] bytes = bitmap.CopyRegionToBytes(sX - xStart, sY, realWidth, partHeight);

                        Draw(sX, sY, eX - sX, eY - sY, bytes);
                    }
                }

            for (int y = 0; y < bitmapHeight; y++)
                for (int x = 0; x < bitmapWidth; x++)
                    Canvas[x + xStart, y] = bitmap[x, y];
        }
        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    }
}