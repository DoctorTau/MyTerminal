using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ClassTerminal
{
    /// <summary>
    /// Class which provides acces to directories and works with them.
    /// </summary>/
    public class Terminal
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(path: "C:\\");
        bool isDirectory = true;
        //Checks if cur pos is in Drives.
        FileInfo bufferFile = null;
        //Buffer file for coping.
        bool isCut = false;
        // Type of coping: copy or cut.
        List<string> encodings = new List<string>(new string[] { "UTF-8", "ASCII", "Unicode" });
        // Encodings that can be use in the terminal.

        /// <summary>
        /// Constructor for Terminal which sets starting directory. 
        /// </summary>
        /// <param name="path">starting directory for terminal.</param>
        public Terminal(string path)
        {
            currentDirectory = new DirectoryInfo(path: path);
        }

        /// <summary>
        /// Gives user an information about his current working directory. 
        /// </summary>
        public string GetCurrentDirectory()
        {
            if (this.isDirectory)
            {
                return (this.currentDirectory.FullName);
            }
            else
            {
                return ("Your Computer.");
            }
        }

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
            Console.WriteLine("cut {file} - adds file to the buffer and  deletes it after pasting.\n");
            Console.WriteLine("paste - paste file from the buffer. If type was cutting that file would stay in the buffer, otherwise buffer would be cleaned.\n");
            Console.WriteLine("delete {file} - delete a file in current directory.\n");
            Console.WriteLine("create {file} {encode} - creates a new file in choosed encode.  Default encoding is UTF-8.\n");
            Console.WriteLine("concat {file 1} {file 2} - concats 2 files into 1 and print it to the console.\n");
            Console.WriteLine("clear - clears the console.\n");
        }

        /// <summary>
        /// Prints  elements of current directory.
        /// </summary>
        private void PrintElements()
        {
            // Prints directories
            foreach (var dirInfo in currentDirectory.GetDirectories())
            {
                try
                {
                    Console.Write("[" + dirInfo.Name + "]" + "\n");
                }
                catch (System.UnauthorizedAccessException)
                {
                    Console.Write("[Access error]\n");
                }

            }

            // Prints files
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
        /// Prints list of drives. 
        /// </summary>
        private void PrintDrives()
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                Console.WriteLine("[" + driveInfo.Name + "]");
            }
        }

        /// <summary>
        /// Checks is directory is Drive
        /// </summary>
        /// <param name="drive">Name of directory to check.</param>
        /// <returns>True if is drive, false otherwise.</returns>
        private bool CheckIsDrive(string drive)
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                if (drive == driveInfo.Name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Changes current directory.
        /// </summary>
        /// <param name="dirName">Name of new directory.</param>
        public void ChangeDirectory(string dirName)
        {
            if (dirName == "..")
            {
                // Check if user go up to drives.
                if (CheckIsDrive(this.currentDirectory.FullName))
                {
                    this.isDirectory = false;
                    return;
                }
                this.currentDirectory = this.currentDirectory.Parent;
                return;
            }
            //Go to drive.
            if (!this.isDirectory)
            {
                if (CheckIsDrive(dirName))
                {
                    this.currentDirectory = new DirectoryInfo(path: dirName);
                    return;
                }
                Console.WriteLine("Incorect drive name.");
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(this.currentDirectory.FullName + dirName);
            if (dir.Exists)
            {
                this.currentDirectory = dir;
                return;
            }
            Console.WriteLine("There is no such directory");
        }

        /// <summary>
        /// Prints list of element if it is a directory, otherwise prints list of drives. 
        /// </summary>
        public void ListOfElements()
        {
            if (this.isDirectory)
            {
                PrintElements();
                return;
            }
            PrintDrives();
        }

        /// <summary>
        /// Check is inputed encodeing correct. 
        /// </summary>
        /// <param name="encoding">Inputed encodeing.</param>
        /// <returns>True if input is correct, false otherwise.</returns>
        public bool CheckEncoding(string encoding)
        {
            if (this.encodings.Find(findEl => findEl == encoding) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prints text file to console. Can use 3 different Encodings: UTF-8, ASCII and Unicode. 
        /// </summary>
        /// <param name="filename">Name of printing file.</param>
        /// <param name="encoding">Choosed encoding, default is UTF-8.</param>
        public void OpenTextFile(string filename, string encoding = "UTF-8")
        {
            Encoding encode;
            switch (encoding)
            {
                case "UTF-8":
                    encode = Encoding.UTF8;
                    break;
                case "ASCII":
                    encode = Encoding.ASCII;
                    break;
                case "Unicode":
                    encode = Encoding.Unicode;
                    break;
                default:
                    Console.WriteLine("Incorrect encoding");
                    return;
            }
            FileInfo fileInfo = new FileInfo(this.currentDirectory.FullName + "\\" + filename);
            if (fileInfo.Exists)
            {
                Console.WriteLine(File.ReadAllText(fileInfo.FullName), encode);
                return;
            }
            Console.WriteLine("Incorrect filename");
        }

        /// <summary>
        /// Add file to the file buffer. 
        /// </summary>
        /// <param name="filename">Name of coping file.</param>
        public void CopyFile(string filename)
        {
            FileInfo fileInfo = new FileInfo(this.currentDirectory + "\\" + filename);
            if (fileInfo.Exists)
            {
                this.bufferFile = fileInfo;
                Console.WriteLine("File copied");
                return;
            }
            Console.WriteLine("Incorrect filename");
        }

        /// <summary>
        /// Add file to the buffer. And turn cut flag on. 
        /// </summary>
        /// <param name="filename">Name of cutting file.</param>
        public void CutFile(string filename)
        {
            FileInfo fileInfo = new FileInfo(this.currentDirectory + "\\" + filename);
            if (fileInfo.Exists)
            {
                this.bufferFile = fileInfo;
                Console.WriteLine("File copied");
                this.isCut = true;
                return;
            }
            Console.WriteLine("Incorrect filename");
        }

        /// <summary>
        /// Paste file from buffert to current directory. 
        /// </summary>
        public void PasteFile()
        {
            if (this.bufferFile != null)
            {
                if (this.isCut)
                {
                    this.bufferFile.MoveTo(this.currentDirectory.FullName);
                    isCut = false;
                    this.bufferFile = null;
                }
                else
                {
                    this.bufferFile.CopyTo(this.currentDirectory.FullName + "\\" + this.bufferFile.Name, true);
                }
                Console.WriteLine("File pasted");
            }
            else
            {
                Console.WriteLine("No file in buffer");
            }
        }

        /// <summary>
        /// Delete file by filename. 
        /// </summary>
        /// <param name="filename">Name of the deliting file.</param>
        public void DeleteFile(string filename)
        {
            FileInfo fileInfo = new FileInfo(this.currentDirectory + "\\" + filename);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                Console.WriteLine("File deleted");
                return;
            }
            Console.WriteLine("Incorrect filename");
        }

        public bool AskYesNo()
        {
            Console.WriteLine("Are you sure? (y/n)");
            string answer = Console.ReadLine();
            if (answer == "y")
                return true;
            return false;
        }

        /// <summary>
        /// Create text file and write some text there. User can choose one of the 3 encodings: UTF-8, ASCII, Unicode.
        /// </summary>
        /// <param name="filename">Name of creating file.</param>
        /// <param name="encoding">Choosed encoding.</param>
        public void CreateTextFile(string filename, string encoding = "UTF-8")
        {
            Encoding encode;
            switch (encoding)
            {
                case "UTF-8":
                    encode = Encoding.UTF8;
                    break;
                case "ASCII":
                    encode = Encoding.ASCII;
                    break;
                case "Unicode":
                    encode = Encoding.Unicode;
                    break;
                default:
                    Console.WriteLine("Incorrect encoding");
                    return;
            }
            Console.WriteLine("Enter your text:");
            string text = Console.ReadLine();
            using (StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create), encode))
            {
                sw.Write(text);
            }
            File.Move(Directory.GetCurrentDirectory() + "\\" + filename, this.currentDirectory.FullName + "\\" + filename);
            Console.WriteLine("File successfully created.");

        }

        /// <summary>
        /// Gets 2 filenames and print concated files.
        /// </summary>
        /// <param name="firstFileName">First file name.</param>
        /// <param name="secondFileName">Second file name.</param>
        public void Concat(string firstFileName, string secondFileName)
        {
            FileInfo fileInfo1 = new FileInfo(this.currentDirectory.FullName + "\\" + firstFileName);
            FileInfo fileInfo2 = new FileInfo(this.currentDirectory.FullName + "\\" + secondFileName);
            if (fileInfo1.Exists && fileInfo2.Exists)
            {
                this.OpenTextFile(firstFileName);
                this.OpenTextFile(secondFileName);
                return;
            }
            Console.WriteLine("Incorrect filename");
        }

    }
}