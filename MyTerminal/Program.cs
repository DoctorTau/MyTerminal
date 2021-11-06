using System;
using ClassTerminal;
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

        public static List<string> CutQuotesInString(string inStr)
        {
            char[] charsInString = inStr.ToCharArray();
            bool quoteFlag = false;

            for (int i = 0; i < charsInString.Length; i++)
            {
                if (charsInString[i] == '"')
                {
                    quoteFlag = !quoteFlag;
                }
                if (!quoteFlag && charsInString[i] == ' ')
                {
                    charsInString[i] = '|';
                }
            }

            List<string> returnsList = new List<string>((new string(charsInString)).Split('|'));
            for (int i = 0; i < returnsList.Count; i++)
            {
                returnsList[i] = returnsList[i].Trim('"');
            }

            return returnsList;
        }

        static void Main(string[] args)
        {
            Terminal terminal = new Terminal("C:\\");
            string input = Console.ReadLine();
            while (input != "quit")
            {
                List<string> inputComands = CutQuotesInString(input);
                string command = inputComands[0];
                string encoding = null;
                if (inputComands.Count > 1)
                    inputComands.RemoveAt(0);
                if ((inputComands.Count > 1) && terminal.CheckEncoding(inputComands[inputComands.Count - 1]) == true)
                {
                    encoding = inputComands[inputComands.Count - 1];
                    inputComands.RemoveAt(inputComands.Count - 1);
                }
                switch (command)
                {
                    case "help":
                        terminal.PrintComandsInfo();
                        break;
                    case "gd":
                        Console.WriteLine(terminal.GetCurrentDirectory());
                        break;
                    case "cd":
                        terminal.ChangeDirectory(ConcatStrings(inputComands));
                        break;
                    case "ls":
                        terminal.ListOfElements();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "copy":
                        terminal.CopyFile(ConcatStrings(inputComands));
                        break;
                    case "cut":
                        terminal.CutFile(ConcatStrings(inputComands));
                        break;
                    case "paste":
                        terminal.PasteFile();
                        break;
                    case "delete":
                        if (terminal.AskYesNo())
                            terminal.DeleteFile(ConcatStrings(inputComands));
                        break;
                    case "open":
                        if (encoding != null)
                            terminal.OpenTextFile(ConcatStrings(inputComands), encoding);
                        else
                            terminal.OpenTextFile(ConcatStrings(inputComands));
                        break;
                    case "create":
                        if (encoding != null)
                            terminal.CreateTextFile(ConcatStrings(inputComands), encoding);
                        else
                            terminal.CreateTextFile(ConcatStrings(inputComands));
                        break;
                    case "concat":
                        if (inputComands.Count != 2)
                        {
                            terminal.PrintError("Invalid files");
                            break;
                        }
                        Console.WriteLine(inputComands[0], inputComands[1]);
                        terminal.Concat(inputComands[0], inputComands[1]);
                        break;
                    case "filesBy":
                        if (inputComands.Count != 1)
                        {
                            terminal.PrintError("Ivalid mask");
                            break;
                        }
                        terminal.GetMaskedFiles(inputComands[0]);
                        break;
                    default:
                        terminal.PrintError("Unkcown command!");
                        break;
                }
                input = Console.ReadLine();
            }
        }
    }
}
