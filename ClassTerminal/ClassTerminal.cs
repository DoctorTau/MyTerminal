using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security;

namespace ClassTerminal
{
    /// <summary>
    /// Class which provides acces to directories and works with them.
    /// </summary>/
    public partial class Terminal
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(path: "C:\\");
        //List of available comands.
        List<string> commands = new List<string>(new string[] { "gd", "cd", "ls", "open", "copy", "cut", "paste", "delete", "create", "concat", "clear", "filesBy" });
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
                return (this.currentDirectory.FullName);
            return ("");
        }

        /// <summary>
        /// Returns list of command. 
        /// </summary>
        /// <returns>List with commands.</returns>
        public List<string> GetAvailableComands()
        {
            return this.commands;
        }


        /// <summary>
        /// Returns string with directories names. 
        /// </summary>
        /// <returns>Names of directories.</returns>
        public List<string> GetDirectoriesInCurrentPosition()
        {
            List<string> dirNames = new List<string>();
            // Prints directories
            foreach (var dirInfo in currentDirectory.GetDirectories())
            {
                try
                {
                    dirNames.Add(dirInfo.Name);
                }
                catch (System.UnauthorizedAccessException)
                {
                    dirNames.Add("[Acces error]");
                }

            }
            return dirNames;
        }

        /// <summary>
        /// Returns string with filenames.
        /// </summary>
        /// <returns>Names of files.</returns>
        public List<string> GetFilesInCurrentPosition()
        {
            List<string> fileNames = new List<string>();
            foreach (var fileInfo in currentDirectory.GetFiles())
            {
                try
                {
                    fileNames.Add(fileInfo.Name);
                }
                catch (IOException)
                {
                    fileNames.Add("File access error");
                }

            }

            return fileNames;
        }

        /// <summary>
        /// Returns list with Drives names.  
        /// </summary>
        /// <returns>List of Drives names.</returns>
        public List<string> GetDrives()
        {
            List<string> drives = new List<string>();
            foreach (var driveInfo in DriveInfo.GetDrives())
                drives.Add(driveInfo.Name.TrimEnd('\\'));
            return drives;
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
            // If line is a full path change current directory to it.
            if (Path.IsPathRooted(dirName))
            {
                DirectoryInfo absPath = new DirectoryInfo(path: dirName);
                if (absPath.Exists)
                {
                    this.currentDirectory = absPath;
                    this.isDirectory = true;
                    return;
                }
            }
            //Go to drive.
            if (!this.isDirectory)
            {
                if (CheckIsDrive(dirName))
                {
                    this.currentDirectory = new DirectoryInfo(path: dirName);
                    isDirectory = true;
                    return;
                }
                PrintError("Incorect drive name.");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(this.currentDirectory.FullName + dirName);
            if (dir.Exists)
            {
                this.currentDirectory = dir;
                return;
            }
            PrintError("There is no such directory");
        }

        public bool GetIsDirectory()
        {
            return this.isDirectory;
        }

        /// <summary>
        /// Check is inputed encodeing correct. 
        /// </summary>
        /// <param name="encoding">Inputed encodeing.</param>
        /// <returns>True if input is correct, false otherwise.</returns>
        public bool CheckEncoding(string encoding)
        {
            if (this.encodings.Find(findEl => findEl == encoding) != null)
                return true;
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
                    PrintError("Incorrect encoding");
                    return;
            }
            FileInfo fileInfo = new FileInfo(this.currentDirectory.FullName + "\\" + filename);
            if (fileInfo.Exists)
            {
                Console.WriteLine(File.ReadAllText(fileInfo.FullName), encode);
                return;
            }
            PrintError("Incorrect filename");
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
                PrintSuccessMessage("File copied");
                return;
            }
            PrintError("Incorrect filename");
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
                PrintSuccessMessage("File copied");
                this.isCut = true;
                return;
            }
            PrintError("Incorrect filename");
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
                    this.bufferFile.CopyTo(this.currentDirectory.FullName + "\\" + this.bufferFile.Name, true);
                PrintSuccessMessage("File pasted");
            }
            else
                PrintError("No file in buffer");
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
                PrintSuccessMessage("File deleted");
                return;
            }
            PrintError("Incorrect filename");
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
                    PrintError("Incorrect encoding");
                    return;
            }
            Console.WriteLine("Enter your text:");
            string text = Console.ReadLine();
            using (StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create), encode))
                sw.Write(text);
            File.Move(Directory.GetCurrentDirectory() + "\\" + filename, this.currentDirectory.FullName + "\\" + filename);
            PrintSuccessMessage("File successfully created.");

        }

        /// <summary>
        /// Gets 2 filenames and print concated files.
        /// </summary>
        /// <param name="firstFileName">First file name.</param>
        /// <param name="secondFileName">Second file name.</param>
        public void Concat(List<string> filesToContac)
        {
            if (!CheckAllFiles(filesToContac))
                PrintError("Incorrect filenames");
            foreach (string fileName in filesToContac)
            {
                FileInfo fileInfo = new FileInfo(this.currentDirectory.FullName + "\\" + fileName);
                this.OpenTextFile(fileName);
            }
        }

        public bool CheckAllFiles(List<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                FileInfo fileInfo = new FileInfo(this.currentDirectory.FullName + "\\" + fileName);
                if (!fileInfo.Exists)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Prints file by mask. 
        /// </summary>
        /// <param name="pattern">File mask.</param>
        public void GetMaskedFiles(string pattern)
        {
            string elements = "";
            try
            {
                foreach (var fileInfo in this.currentDirectory.GetFiles(pattern))
                    elements += $"{fileInfo.Name} ({fileInfo.Length / 8}B)\n";
                PrintListOfFiles(elements);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case SecurityException:
                        PrintError("Acces Error");
                        break;
                    case ArgumentException:
                        PrintError("Wrong search pattern");
                        break;
                }
            }
        }

    }
}