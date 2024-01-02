using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

public class Hotbar
{
    public Hotbar(OS os)
    {
        OS = os;
    }

    public const int IconSize = 32;
    public const int Width = IconSize + 2;
    public const int Height = Display.HEIGHT;

    public readonly OS OS;
    public int SelectedIndex = -1;

    private MemoryBitmap hotbarBitmap;
    private MemoryBitmap defaultWindowBitmap;
    private MemoryBitmap unselectedCapBitmap;
    private MemoryBitmap selectedCapBitmap;

    public void InitDraw()
    {
        hotbarBitmap = new(Width, Height);
        hotbarBitmap.Fill(OS.Palette.Hotbar);

        defaultWindowBitmap = new(OS.PhysDisplay.Width, OS.PhysDisplay.Height);
        defaultWindowBitmap.Fill(OS.Palette.Background);

        unselectedCapBitmap = new(2, 32);
        unselectedCapBitmap.Fill(OS.Palette.Hotbar);

        selectedCapBitmap = new(2, 32);
        selectedCapBitmap.Fill(OS.Palette.SelectedCapHotbar);
    }

    public Hotbar DrawBackgroud()
    {
        OS.Display.DrawBitmap(hotbarBitmap, 0, 0);

        return this;
    }

    public void Select(int index)
    {
        if (SelectedIndex != -1)
            Unselect();

        if (index >= OS.Apps.Count)
            return;

        SelectedIndex = index;

        OS.Display.DrawBitmap(selectedCapBitmap, IconSize, SelectedIndex * IconSize);

        OS.OpenApp(OS.Apps[SelectedIndex]);
    }

    public void Unselect()
    {
        OS.Display.DrawBitmap(unselectedCapBitmap, IconSize, SelectedIndex * IconSize);

        for (int y = 0; y < OS.PhysDisplay.Height; y++)
            for (int x = 0; x < Width; x++)
                defaultWindowBitmap[x, y] = OS.Canvas[x, y];
        OS.Display.FillBitmapByParts(defaultWindowBitmap);

        SelectedIndex = -1;
        OS.CloseApp();
    }
}