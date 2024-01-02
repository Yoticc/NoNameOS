using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringAPI;
public enum Commands
{
    Reset = 101,
    Clear = 102,
    ToBlack = 103,
    Disable = 108,
    Enable = 109,
    SetBrightness = 110,
    SetOrientation = 121,
    DrawBitmap = 197,
    Test = 255
}