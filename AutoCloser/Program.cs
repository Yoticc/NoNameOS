using System.Diagnostics;

while (true)
{
    Thread.Sleep(1000);
    var processes = Process.GetProcessesByName("NoNameOS");

    if (processes.Length == 0)
        continue;

    var process = processes[0];
    Thread.Sleep(20000);
    process.Kill();
}