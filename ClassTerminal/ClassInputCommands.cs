using System;
using System.Collections.Generic;
using System.Text;
using ClassTerminal;

namespace ClassInputCommands
{
    public static class InputCommands
    {
        static List<string> lastCommands = new List<string>();

        public static List<string> InputComds(Terminal terminal)
        {

            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo inputKey;
            int curPos = 0, historyPos = 0;
            string bufferForInput = "";

            inputKey = Console.ReadKey();
            while (inputKey.Key != ConsoleKey.Enter)
            {
                switch (inputKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (curPos > sb.Length)
                            break;
                        curPos++;
                        Console.CursorLeft++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (curPos == 0)
                            break;
                        curPos--;
                        Console.CursorLeft--;
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
                        UpdateLine(sb);
                        curPos = sb.Length;
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
                        UpdateLine(sb);
                        curPos = sb.Length;
                        break;
                    case ConsoleKey.Backspace:
                        if (curPos == 0)
                            break;
                        sb.Remove(--curPos, 1);
                        UpdateLine(sb);
                        break;
                    case ConsoleKey.Delete:
                        if (curPos >= sb.Length)
                            break;
                        sb.Remove(curPos, 1);
                        UpdateLine(sb);
                        break;
                    case ConsoleKey.Tab:
                        sb = AutoCompleteMethod(sb, terminal);
                        UpdateLine(sb);
                        curPos = sb.Length;
                        Console.CursorLeft = curPos;
                        break;
                    default:
                        sb.Insert(curPos++, inputKey.KeyChar);
                        UpdateLine(sb);
                        break;
                }
                Console.CursorLeft = curPos;
                inputKey = Console.ReadKey();
            }
            List<string> returnsList = new List<string>(CutQuotesInString(sb.ToString()));
            return returnsList;
        }

        /// <summary>
        /// Clears input line and puts new text there. 
        /// </summary>
        /// <param name="sb">StringBuilder with new text.</param>
        public static void UpdateLine(StringBuilder sb)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\r");
            Console.Write(new string(' ', Console.BufferWidth - 1));
            Console.Write("\r");
            Console.Write(sb.ToString());
            Console.ResetColor();
        }

        public static StringBuilder AutoCompleteMethod(StringBuilder sb, Terminal terminal)
        {
            List<string> inputLine = TrimStringElements(CutQuotesInString(sb.ToString()));
            List<string> matchedLines = new List<string>();
            if (inputLine.Count == 1)
            {
                matchedLines = GetMachedLines(inputLine[0], terminal.GetAvailableComands());
            }
            else if (inputLine.Count > 1 && inputLine[0] == "cd")
            {
                if (terminal.GetIsDirectory())
                    matchedLines = GetMachedLines(inputLine[inputLine.Count - 1], terminal.GetDirectoriesInCurrentPosition());
                else
                    matchedLines = GetMachedLines(inputLine[inputLine.Count - 1], terminal.GetDrives());
            }
            else if (inputLine.Count > 1)
            {
                matchedLines = GetMachedLines(inputLine[inputLine.Count - 1], terminal.GetFilesInCurrentPosition());
            }
            else
            {
                return sb;
            }

            if (matchedLines.Count > 1)
            {
                Console.WriteLine();
                PrintElementsOfList(matchedLines);
                return sb;
            }

            if (matchedLines.Count == 1)
            {
                inputLine[inputLine.Count - 1] = matchedLines[0];
                sb = new StringBuilder(ConcatStrings(inputLine) + " ");
                return sb;
            }

            return sb;
        }

        /// <summary>
        /// Prints elements of list with strings in one row. 
        /// </summary>
        /// <param name="elements">List of strings.</param>
        public static void PrintElementsOfList(List<string> elements)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (var element in elements)
            {
                Console.Write(element + " ");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public static List<string> TrimStringElements(List<string> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i] = elements[i].Trim(' ');
            }
            return elements;
        }

        /// <summary>
        /// Returns list of mached lines. 
        /// </summary>
        /// <param name="line">Line to find.</param>
        /// <param name="linesForSearch">List of lines to search in.</param>
        /// <returns>List of mached lines.</returns>
        public static List<string> GetMachedLines(string line, List<string> linesForSearch)
        {
            int lenOfInputedLine = line.Length;
            List<string> matchedLines = new List<string>();
            foreach (var lineInList in linesForSearch)
            {
                if (lineInList.Length >= lenOfInputedLine && line == lineInList.Substring(0, lenOfInputedLine))
                {
                    matchedLines.Add(lineInList);
                }
            }

            return matchedLines;
        }

        /// </summary>
        /// <param name="inStr">String to split.</param>
        /// <returns>List of separated strings.</returns>
        public static List<string> CutQuotesInString(string inStr)
        {
            char[] charsInString = inStr.ToCharArray();
            bool quoteFlag = false;

            for (int i = 0; i < charsInString.Length - 1; i++)
            {
                if (charsInString[i] == '"')
                {
                    quoteFlag = !quoteFlag;
                }
                if (!quoteFlag && charsInString[i] == ' ' && charsInString[i + 1] != ' ')
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
    }
}