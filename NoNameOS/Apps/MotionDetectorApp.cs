using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

namespace NoNameOS.Apps;
public class MotionDetectorApp() : App("Motion Detector")
{
    public override MemoryBitmap GetIcon()
    {
        using var bitmap = NewGDIBitmapForIcon;
        using var g = Graphics.FromImage(bitmap);

        g.FillRectangle(new SolidBrush(Color.FromArgb(53, 53, 53)), IconGDIRect);

        var font = new Font("MS UI Gothic", 22);
        g.DrawString("M", font, new SolidBrush(Color.Silver), 1, 1);

        return MemoryBitmap.FromGDIBitmap(bitmap);
    }
}