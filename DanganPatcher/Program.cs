using DanganPatcher;
using System.Diagnostics;

public partial class Program
{
    static string steamappid = "555950";

    static bool startGameExe()
    {
        if (Process.GetProcessesByName("game").Length > 0)
            return true;
        Program.PrintFancy(ConsoleColor.Cyan, "game.exe not detected, starting manually...");
        Process.Start("explorer", $"steam://run/{steamappid}");
        int attempt = 0;
        while (Process.GetProcessesByName("game").Length == 0)
        {
            attempt++;
            if (attempt > 10)
                return false;
            Thread.Sleep(1000);
        }
        return true;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("hello!");
        if (!startGameExe())
        {
            Console.WriteLine("Failed to start patcher, game was not open, and could not launch game");
            Console.ReadLine();
            return;
        }
        Process proc = Process.GetProcessesByName("game")[0];
        Patcher patcher = new Patcher(proc);
        PatchConfig config = PatchConfig.LoadConfig();
        config.ApplyPatches(patcher);

        Program.PrintFancy(ConsoleColor.Green,"Done applying patches");
        Console.WriteLine("Press enter to close!");
        Console.ReadLine();
    }

    public static void PrintFancy(ConsoleColor color, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = oldColor;
    }
}