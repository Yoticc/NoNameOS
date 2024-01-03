using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TuringAPI;

namespace NoNameOS.Kernel.DisplayManagement.Windows;
public unsafe class WinDisplay : IDisplay
{
    Form window;
    Graphics g;

    [STAThread]
    public void Init()
    {
        Application.EnableVisualStyles();
        window = new Form()
        {
            Size = new Size(Display.WIDTH, Display.HEIGHT),
            FormBorderStyle = FormBorderStyle.None
        };
        g = window.CreateGraphics();

        window.Load += Window_Load;

        Application.Run(window);
    }

    void Window_Load(object? sender, EventArgs e)
    {
        window.Location = new Point(2700, 680);
    }

    public override void Draw(int x, int y, int width, int height, byte[] bytes)
    {
        var bitmap = new Bitmap(width, height);
        fixed (byte* bytesPtr = bytes)
        {
            var ptr = (MemoryPixel*)bytesPtr;
            for (int cx = 0; cx < width; cx++)
                for (int cy = 0; cy < height; cy++)
                {
                    var rx = x + cx;
                    var ry = y + cy;
                    var pixel16 = ptr[cy * width + cx];
                    var pixel24 = pixel16.ToGDIColor();
                    bitmap.SetPixel(cx, cy, pixel24);
                }
        }
        g.DrawImageUnscaled(bitmap, x, y);
    }
}