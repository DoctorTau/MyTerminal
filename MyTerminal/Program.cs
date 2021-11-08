using System;
using ClassTerminal;
using ClassInputCommands;
using System.Collections.Generic;
namespace MyTerminal
{
    class Program
    {
        /// <summary>
        /// Concats a banch of string at one.
        /// </summary>
        /// <param name="inStr">Massive of strings.</param>
        /// <returns>One string which consists of string form inStr.</returns>
        public static string ConcatStrings(List<string> inStr)
        {
            string output = "";

            foreach (string line in inStr)
            {
                output += line + " ";
            }

            return output.TrimEnd();
        }


        public static bool CheckCountOfArgumets(List<string> commands, int countOfArgumets)
        {
            return commands.Count == countOfArgumets;
        }

        public static List<string> TrimQuotes(List<string> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                commands[i] = commands[i].Trim('"');
            }

            return commands;
        }

        static void Main(string[] args)
        {
            bool workFlag = true;
            Terminal terminal = new Terminal("C:\\");
            // string input = Console.ReadLine();
            while (workFlag)
            {
                List<string> inputCommands = InputCommands.InputComds(terminal);
                Console.WriteLine(inputCommands[0]);
                string command = inputCommands[0];
                string encoding = null;
                if (inputCommands.Count > 1)
                    inputCommands.RemoveAt(0);
                if ((inputCommands.Count > 1) && terminal.CheckEncoding(inputCommands[inputCommands.Count - 1]) == true)
                {
                    encoding = inputCommands[inputCommands.Count - 1];
                    inputCommands.RemoveAt(inputCommands.Count - 1);
                }
                inputCommands = TrimQuotes(inputCommands);
                switch (command)
                {
                    case "help":
                        terminal.PrintComandsInfo();
                        break;
                    case "gd":
                        string curDrirectory = terminal.GetCurrentDirectory();
                        terminal.PrintSuccessMessage(curDrirectory == "" ? "Your Computer" : curDrirectory);
                        break;
                    case "cd":
                        if (CheckCountOfArgumets(inputCommands, 1))
                            terminal.ChangeDirectory(ConcatStrings(inputCommands));
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "ls":
                        terminal.PrintElementInCurentPosition();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "copy":
                        if (CheckCountOfArgumets(inputCommands, 1))
                            terminal.CopyFile(ConcatStrings(inputCommands));
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "cut":
                        if (CheckCountOfArgumets(inputCommands, 1))
                            terminal.CutFile(ConcatStrings(inputCommands));
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "paste":
                        terminal.PasteFile();
                        break;
                    case "delete":
                        if (CheckCountOfArgumets(inputCommands, 1))
                        {
                            if (terminal.AskYesNo())
                                terminal.DeleteFile(ConcatStrings(inputCommands));
                        }
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "open":
                        if (CheckCountOfArgumets(inputCommands, 1))
                        {
                            if (encoding != null)
                                terminal.OpenTextFile(ConcatStrings(inputCommands), encoding);
                            else
                                terminal.OpenTextFile(ConcatStrings(inputCommands));
                        }
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "create":
                        if (CheckCountOfArgumets(inputCommands, 1))
                        {
                            if (encoding != null)
                                terminal.CreateTextFile(ConcatStrings(inputCommands), encoding);
                            else
                                terminal.CreateTextFile(ConcatStrings(inputCommands));
                        }
                        else
                            terminal.PrintError("Invalid argument");
                        break;
                    case "concat":
                        if (CheckCountOfArgumets(inputCommands, 2))
                        {
                            terminal.Concat(inputCommands[0], inputCommands[1]);
                            break;
                        }
                        else
                            terminal.PrintError("Invalid files");
                        break;
                    case "filesBy":
                        if (CheckCountOfArgumets(inputCommands, 1) && inputCommands[0][0] == '*')
                        {
                            terminal.GetMaskedFiles(inputCommands[0]);
                            break;
                        }
                        else
                            terminal.PrintError("Ivalid mask");
                        break;
                    case "quit":
                        workFlag = false;
                        break;
                    default:
                        terminal.PrintError("Unkcown command");
                        break;
                }
                // input = Console.ReadLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
