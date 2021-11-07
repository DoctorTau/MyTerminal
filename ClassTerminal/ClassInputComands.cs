using System;
using System.Collections.Generic;
using System.Text;
using ClassTerminal;

namespace ClassInputComands
{
    static class InputComands
    {
        static List<string> lastCommands = new List<string>();

        public static List<string> InputComds(Terminal terminal)
        {

            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo inputKey;
            int curPos = 0, historyPos = 0;
            string bufferForInput = "";

            Console.ForegroundColor = ConsoleColor.Cyan;

            inputKey = Console.ReadKey();
            while (inputKey.Key != ConsoleKey.Enter)
            {
                switch (inputKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (curPos > sb.Length + 1)
                            break;
                        curPos++;
                        Console.CursorLeft--;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (curPos == 0)
                            break;
                        curPos--;
                        Console.CursorLeft++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (historyPos == lastCommands.Count)
                            break;
                        if (historyPos == 0)
                        {
                            bufferForInput = sb.ToString();
                            sb = new StringBuilder(lastCommands[0]);
                        }
                        else
                        {
                            sb = new StringBuilder(lastCommands[historyPos]);
                        }
                        historyPos++;
                        curPos = sb.Length;
                        UpdateLine(sb);
                        break;
                    case ConsoleKey.DownArrow:
                        if (historyPos == 0)
                            break;
                        if (historyPos == 1)
                        {
                            sb = new StringBuilder(bufferForInput);
                        }
                        else
                        {
                            sb = new StringBuilder(lastCommands[historyPos - 1]);
                        }
                        historyPos--;
                        curPos = sb.Length;
                        UpdateLine(sb);
                        break;
                    case ConsoleKey.Backspace:
                        if (curPos == 0)
                            break;
                        sb.Remove(--curPos, 1);
                        break;
                    case ConsoleKey.Delete:
                        if (curPos > sb.Length)
                            break;
                        sb.Remove(curPos, 1);
                        break;
                    case ConsoleKey.Tab:
                        break;
                }
            }
            return new List<string>();
        }

        public static void UpdateLine(StringBuilder sb)
        {
            Console.Write("\r");
            Console.Write(new string(' ', Console.BufferWidth));
            Console.Write(sb.ToString());
        }

        public static void CheckForNameMatch(StringBuilder sb)
        {

        }


    }
}