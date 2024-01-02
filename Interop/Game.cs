using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Game
{
    public static string Name;
    public static Process Process;
    public static nint HWnd;
    public static bool IsFocus;

    public delegate void FoundHandler();
    public static event FoundHandler? Found;

    public static void SetGame(string name)
    {
        Name = name;
        Process[] processes;

        while (true)
        {
            processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                Thread.Sleep(100);
                continue;
            }
            Process = processes[0];
            break;
        }

        new Thread(() =>
        {
            while (true)
            {
                HWnd = Process.MainWindowHandle;
                IsFocus = HWnd == GetForegroundWindow();
                Thread.Sleep(5);
            }
        }).Start();

        Found?.Invoke();
    }

    public static void PressKey(Keys key)
    {

    }
}