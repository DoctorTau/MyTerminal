using System;
using ClassTerminal;
namespace MyTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Terminal terminal = new Terminal("C:\\");
            terminal.PrintElements();
            terminal.ChangeDirectory("games");
            Console.WriteLine();
            terminal.PrintElements();
        }
    }
}
