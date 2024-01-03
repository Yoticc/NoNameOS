using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNameOS.Utils;
public static class SugarExtensions
{
    public static int FindLastCharIndex(this string str, char c)
    {
        var foundAnother = false;
        for (int i = str.Length - 1; i >= 0; i--)
            if (str[i] == c && foundAnother)
                return i;
            else foundAnother = true;

        return -1;
    }
}