using NoNameOS.Apps;
using NoNameOS.Apps.Terminal;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TuringAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            var os = new OS(
                new()
                {
                    ShowLogo = false,
                    //ShowVirtualWindowsDisplay = true
                },
                new InfoApp(),
                new TerminalApp(),
                new MotionDetectorApp()
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Console.ReadLine();
    }
}