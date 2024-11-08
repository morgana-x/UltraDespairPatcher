using DanganPatcher;
using System.Diagnostics;

public partial class Program
{
    static string steamappid = "555950";
    public static void Main(string[] args)
    {
        Console.WriteLine("hello!");
        while (Process.GetProcessesByName("game").Length == 0)
        {
            Console.WriteLine("You need to have the game open to use this patcher!");
            Thread.Sleep(500);
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