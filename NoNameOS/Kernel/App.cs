using NoNameOS.Kernel.DisplayManagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

public abstract class App
{
    public App(string name)
    {
        Name = name;
        SetIcon(GetIcon());
    }

    public readonly string Name;
    public MemoryBitmap Icon;
    public VirtualDisplay Display;
    public OS OS;

    public Bitmap NewGDIBitmapForIcon => new(Hotbar.IconSize, Hotbar.IconSize);
    public Rectangle IconGDIRect => new(0, 0, Hotbar.IconSize, Hotbar.IconSize);

    public void SetOS(OS os)
    {
        OS = os;
        Display = os.WorkspaceDisplay;
    }

    public void SetIcon(MemoryBitmap icon, bool disposeOld = true)
    {
        if (disposeOld && Icon != null)
            Icon.Dispose();

        Icon = icon;
    }

    public void SetIcon(Bitmap icon, bool disposeOld = true) => SetIcon(MemoryBitmap.FromGDIBitmap(icon), disposeOld);

    public virtual MemoryBitmap GetIcon() => MemoryBitmap.FromGDIBitmap(NewGDIBitmapForIcon);
    public virtual void Init() { }
    public virtual void Open() { }
    public virtual void Close() { }
    public virtual void Update() { }

    public virtual void OnMouseMove(int x, int y) { }
    public virtual void OnMouseClick(bool isLeft) { }
    public virtual void OnLeftMouseClick() { }
    public virtual void OnRightMouseClick() { }
    public virtual void OnKeyDown(Key key) { }
    public virtual void OnKeyUp(Key key) { }

    public virtual void OnExternalMouseMove(int x, int y) { }
    public virtual void OnExternalMouseClick(bool isLeft) { }
    public virtual void OnExternalLeftMouseClick() { }
    public virtual void OnExternalRightMouseClick() { }
    public virtual void OnExternalKeyDown(Key key) { }
    public virtual void OnExternalKeyUp(Key key) { }
}