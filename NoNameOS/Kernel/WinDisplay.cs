using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringAPI;

namespace NoNameOS.Win;
public class WinDisplay : IDisplay
{
    public override void Draw(int x, int y, int width, int height, byte[] bytes)
    {
        throw new NotImplementedException();
    }
}