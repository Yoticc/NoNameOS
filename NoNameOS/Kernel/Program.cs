using NoNameOS.Apps;
using System.Diagnostics;
using TuringAPI;

try
{
    var os = new OS(
        new() {
            ShowLogo = false
        },
        new InfoApp(),
        new MotionDetectorApp()
    );
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.ReadLine();