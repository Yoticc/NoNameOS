using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringAPI;
public unsafe partial class Display
{
    public void Reset() => SendCommand(Commands.Reset);
    public void Reconnect()
    {
        Reset();
        Close();
        Thread.Sleep(5);
        Open();
    }
    public void Clear()
    {
        if (Orientation != Orientations.Portrait)
        {
            Orientations oldOrientation = Orientation;
            SetOrientation(Orientations.Portrait);
            SendCommand(Commands.Clear);
            SetOrientation(oldOrientation);
        }
        else        
            SendCommand(Commands.Clear);
    }
    public void ToBlack() => SendCommand(Commands.ToBlack);
    public void Enable() => SendCommand(Commands.Enable);
    public void Disable() => SendCommand(Commands.Disable);
    public void SetBrightness(double perc) => SendCommand(Commands.SetBrightness, (byte)((1 - perc) * byte.MaxValue));
    public void SetOrientation(Orientations orientation) => SetOrientation(orientation, Width, Height);
    public void SetOrientation(Orientations orientation, int width, int height)
    {
        Write(0, 0, 0, 0, 0,
        (byte)Commands.SetOrientation,
        (byte)((Orientation = orientation) + 100),
        (byte)(width >> 8),
        (byte)(width & 255),
        (byte)(height >> 8),
        (byte)(height & 255), 0, 0, 0, 0, 0);
    }

    public override void Draw(int x, int y, int width, int height, byte[] bytes)
    {
        SendCommand(Commands.DrawBitmap, x, y, x + width - 1, y + height - 1);
        Write(bytes);
    }
}