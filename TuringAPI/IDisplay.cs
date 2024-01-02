using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuringAPI;
// Fuck interfaces, dummkopf oopers 😡
public abstract unsafe class IDisplay
{
    public void Draw(int x, int y, int with, int height, MemoryBitmap bitmap) => Draw(x, y, with, height, bitmap.ByteData);

    public void Draw(int x, int y, int width, int height, void* ptr)
    {
        int count = width * height * sizeof(short);
        byte[] bytes = new byte[count];
        fixed (byte* bytesPtr = bytes)
            Buffer.MemoryCopy(ptr, bytesPtr, count, count);
        Draw(x, y, width, height, bytes);
    }

    public abstract void Draw(int x, int y, int width, int height, byte[] bytes);
}