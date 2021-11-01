using System;
using System.IO;

namespace MyTerminal
{
    /// <summary>
    /// Class which provides acces to  directories and works with them
    /// </summary>/
    class Terminal
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(path: "C:\\");

        /// <summary>
        /// Constructor for Terminal which sets starting directory 
        /// </summary>
        /// <param name="path">starting directory for terminal</param>
        public Terminal(string path)
        {
            currentDirectory = new DirectoryInfo(path: path);
        }

        /// <summary>
        /// Prints  elements of current directory 
        /// </summary>
        public void PrintElements()
        {
            foreach (var dirInfo in currentDirectory.GetDirectories())
            {
                try
                {
                    Console.Write("[" + dirInfo.Name + "]" + "\n");
                }
                catch (System.UnauthorizedAccessException)
                {
                    Console.Write("└───[Access error]\n");
                }

            }

            foreach (var fileInfo in currentDirectory.GetFiles())
            {
                try
                {
                    Console.Write(fileInfo.Name + " (" + (fileInfo.Length / 8) + " B)" + "\n");
                }
                catch (IOException)
                {
                    Console.Write("{File access error}\n");
                }

            }
        }

        /// <summary>
        /// Changes current directory1
        /// </summary>
        /// <param name="fileName">Name of new directory</param>
        public void ChangeDirectory(string fileName)
        {
            foreach (var dirInfo in currentDirectory.GetDirectories())
            {
                if (dirInfo.Name == fileName)
                {
                    this.currentDirectory = new DirectoryInfo(path: dirInfo.FullName);
                    return;
                }
            }
            Console.WriteLine("There is no such directory");
        }



    }
}