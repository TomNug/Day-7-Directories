using System;
using System.Collections.Generic;

class DirectoryNode
{
    string name;
    bool isFolder;
    List<string> children;
    int size;

    public DirectoryNode(string newName, bool newIsFolder, int newSize = 0)
    {
        name = newName;
        isFolder = newIsFolder;
        size = newSize;
    }
}

class Program
{
    public static void Main(string[] args)
    {
        string[] cmds = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day-7-Directories\Day-7-Directories\data_test.txt");

        DirectoryNode home = new DirectoryNode("/", true, 0);

        string currentDir = "";
        foreach (string cmd in cmds)
        {
            if (cmd[0] == '$' && cmd[2] == 'c')
            {
                // $ cd
                if (cmd[5] == '.')
                {
                    // $ cd ..
                    Console.WriteLine("Change out");
                }
                else
                {
                    string dir = cmd.Substring(5);
                    Console.WriteLine(String.Format("Change into {0}", dir));
                }
            }
        }

        // CurrentDirectoryStack

        // For each command

        // Create directory

        // Change to directory

        // Create file




    }
}