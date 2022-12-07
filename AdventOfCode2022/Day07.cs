using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode_2022
{
    internal class Day07 : AoC2022
    {
        internal static void Start()
        {
            // Prompt the user to ask if this is a test
            var lines = TestPrompt(7);

            // Start the stopwatch to track execution time
            var watch = Stopwatch.StartNew();

            // Initialize variables
            var total = 0;
            var home = new Directory("/");
            var currentDirectory = home;

            // Parse the input and create our directory structure
            for (var i = 0; i < lines.Length; i++)
            {
                // Break up our input into pertinent command pieces
                var cmd = lines[i].Split(' ');

                switch (cmd[0])
                {
                    case "$": // Checking for input commands
                        if (cmd[1] == "cd") // "cd" is the only command of consequence, so ignore "ls"
                        {
                            if (cmd[2] == "/") currentDirectory = home;
                            else currentDirectory = currentDirectory.Find(cmd[2]);
                        }
                        break;
                    case "dir": // Check for a new directory to add to the tree
                        if (!currentDirectory.Contains(cmd[1])) currentDirectory.AddSubfolder(new Directory(cmd[1], currentDirectory));
                        break;
                    default: // Default assumes a file size and name was provided
                        int size;
                        if (int.TryParse(cmd[0], out size)) currentDirectory.AddFile(new File(cmd[1], size));
                        break;
                }
            }

            // Add all directory sizes to a Dictionary
            var directorySizes = GenerateSizeDictionary(home);

            // Determine existing free space on device
            var freeSpace = 70000000 - directorySizes[home];
            var neededSpace = 30000000 - freeSpace;

            // Initialize our "delete" directory to "home" to ensure it is the largest directory
            var delete = home;

            // Determine the smallest directory that is still large enough to free up the needed space
            // as well as determining the total size of all directories less than 100MB in size
            foreach(var size in directorySizes)
            {
                if (size.Value <= 100000) total += size.Value;

                if (size.Value >= neededSpace && size.Value < directorySizes[delete]) delete = size.Key;
            }

            // Output results to console
            Console.WriteLine("The total file size of directories less than 100MB is " + total + ".\n");
            Console.WriteLine("The total remaining free space before deletion is " + freeSpace + ".");
            Console.WriteLine("The needed space for the update is " + neededSpace + ".");
            Console.WriteLine("The directory to be deleted is \"" + delete.Name + "\".");
            Console.WriteLine("The size of the directory is " + directorySizes[delete] + ".\n");
            Console.WriteLine("Name".PadRight(35) + "Size");
            PrintTree(home);
            Console.ResetColor();
            Console.WriteLine();
            Summary(watch);
        }

        // Recursively creates a Dictionary containing all directories and their sizes
        private static Dictionary<Directory, int> GenerateSizeDictionary(Directory directory)
        {
            var sizes = new Dictionary<Directory, int>();

            // Add current directory and size
            sizes.Add(directory, directory.GetSize());
            
            // Loop through and recursively obtain dictionaries for each possible subfolder
            foreach(var folder in directory.Subfolders)
            {
                var subsizes = GenerateSizeDictionary(folder); // Recursive call

                // Merge the recursively derived dictionaries into the main
                foreach(var sub in subsizes)
                {
                    sizes.Add(sub.Key, sub.Value);
                }
            }

            return sizes;
        }

        // Print a tree view of the directory
        private static void PrintTree(Directory directory, int depth = 0)
        {
            Console.WriteLine(Indent(depth) + "- " + directory.Name);

            foreach (var file in directory.Files)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write((Indent(depth + 2) + "* " + file.Name + " ").PadRight(35));

                Console.ResetColor();
                Console.WriteLine(file.Size);
            }

            foreach (var sub in directory.Subfolders)
            {
                Console.ResetColor();
                PrintTree(sub, depth + 2);
            }
        }

        // Create an indent for tree depth
        private static string Indent(int depth)
        {
            var result = "";
            for (int i = 0; i < depth; i++) result += " ";
            return result;
        }
    }

    internal class Directory
    {
        public string Name;
        public List<Directory> Subfolders = new List<Directory>();
        public List<File> Files = new List<File>();
        public Directory Parent;

        // Create a "home" directory
        public Directory(string name)
        {
            Name = name;
            Parent = this;
        }

        // Create a subdirectory with a parent
        public Directory(string name, Directory parent)
        {
            Name = name;
            Parent = parent;
        }

        // Add a subfolder to the current directory
        public void AddSubfolder(Directory subfolder)
        {
            Subfolders.Add(subfolder);
        }

        // Add a file to the current directory
        public void AddFile(File file)
        {
            Files.Add(file);
        }

        // Recursively get the full size of all the files and subfolders in this directory
        public int GetSize()
        {
            var result = 0;

            foreach(var folder in Subfolders)
            {
                result += folder.GetSize();
            }

            foreach(var file in Files)
            {
                result += file.Size;
            }

            return result;
        }

        // Checks if the current directory already contains a subfolder with the input name
        public Boolean Contains(string directory)
        {
            foreach(var folder in Subfolders)
            {
                if (folder.Contains(directory)) return true;
            }

            return Subfolders.Exists(d => d.Name == directory);
        }

        // Finds and returns a directory based on either name or returns parent directory is ".." is input
        public Directory Find(string directory)
        {
            if (directory == "..") return Parent;

            return Subfolders.Find(d => d.Name == directory);
        }
    }

    // A simple File object to hold the name and size
    internal class File
    {
        public int Size;
        public string Name;

        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}