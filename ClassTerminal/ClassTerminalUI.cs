using System;
using System.IO;
using System.Collections.Generic;

namespace ClassTerminal
{
    public partial class Terminal
    {

        /// <summary>
        /// Prints information about commands for user.  
        /// </summary>
        public void PrintComandsInfo()
        {
            Console.WriteLine("There are some commands for this terminal:\n");
            Console.WriteLine("gd - prints you current directory\n");
            Console.WriteLine("cd {dirname} - changes your working directory to one of ypur current directory\nWrite '..' to go up\n");
            Console.WriteLine("ls - prints you list of file and dirrectories in your current directory\n");
            Console.WriteLine("open {file} {encode} - opens file in your encode. Defaulten code - UTF-8\n");
            Console.WriteLine("copy {file} - adds file to the buffer.\n");
            Console.WriteLine("copyAll - adds all files and directories to the buffer.\n");
            Console.WriteLine("cut {file} - adds file to the buffer and  deletes it after pasting.\n");
            Console.WriteLine("paste - paste file from the buffer. If type was cutting that file would stay in the buffer, otherwise buffer would be cleaned.\n");
            Console.WriteLine("delete {file} - delete a file in current directory.\n");
            Console.WriteLine("create {file} {encode} - creates a new file in choosed encode.  Default encoding is UTF-8.\n");
            Console.WriteLine("concat \"{file 1}\" \"{file 2}\" ... - concats 2 and more files into 1 and print it to the console.\n");
            Console.WriteLine("clear - clears the console.\n");
            Console.WriteLine("filesBy {*...} - prints file whith your mask.\n");
        }

        /// <summary>
        /// Prints error message to console. 
        /// </summary>
        /// <param name="message">Printing message.</param>
        public void PrintError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Print one string in blue color.
        /// </summary>
        /// <param name="files">String to print.</param>
        public void PrintListOfFiles(string files)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(files);
            Console.ResetColor();
        }

        public void PrintElementInCurentPosition()
        {
            string elements = "";
            if (this.isDirectory)
            {
                foreach (string element in this.GetDirectoriesInCurrentPosition())
                    elements += "[" + element + "]\n";
                foreach (string element in this.GetFilesInCurrentPosition())
                    elements += element + "\n";
            }
            else
            {
                foreach (string element in this.GetDrives())
                    elements += "[" + element + "]\n";
            }
            PrintListOfFiles(elements);
        }

        /// <summary>
        /// Print messege in green color. 
        /// </summary>
        /// <param name="message">String to print.</param>
        public void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

}