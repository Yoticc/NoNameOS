using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Interop
{
    private const string user = "user32";

    [DllImport(user)] public static extern
        short GetKeyState(Keys vKey);
}