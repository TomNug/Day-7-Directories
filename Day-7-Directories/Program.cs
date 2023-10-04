using System;
using System.Collections.Generic;

class DirectoryNode
{
    private string name;
    private bool isFolder;
    private List<string> children;
    private int size;

    public DirectoryNode(string newName, bool newIsFolder, int newSize = 0)
    {
        name = newName;
        isFolder = newIsFolder;
        children = new List<string>();  
        size = newSize;
    }

    public void AddChild(string childName)
    {
        children.Add(childName);
    }

    public int NumberOfChildren()
    {
        return children.Count;
    }

    public List<string> GetChildren()
    {
        return children;
    }
}

class Program
{
    public static void Main(string[] args)
    {
        string[] cmds = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day-7-Directories\Day-7-Directories\data_test.txt");


        DirectoryNode home = new DirectoryNode("/", true, 0);
        Dictionary<string, DirectoryNode> fromNameToDirectory = new Dictionary<string, DirectoryNode>();

        // Starting in home directory
        string currentDir = ""; 
        DirectoryNode newNode = new DirectoryNode("/", true);
        fromNameToDirectory.Add("/", newNode); // Create new node in the dictionary
        
        foreach (string cmd in cmds)
        {
            if (cmd[0] == '$')
            {
                if (cmd[2] == 'c')
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
                        currentDir = dir;
                        Console.WriteLine(String.Format("Change into {0}", dir));
                    }
                }
            } else // Creating a text file or a directory
            {
                // Creating a directory
                if (cmd.Substring(0, 3) == "dir")
                {
                    // Making a new directory
                    string directoryName = cmd.Substring(4); // After "dir " to end is the name
                    newNode = new DirectoryNode(directoryName, true);
                    fromNameToDirectory.Add(directoryName, newNode); // Create new node in the dictionary
                    fromNameToDirectory[currentDir].AddChild(directoryName);
                } else // Creating a file
                {
                    string[] sizeName = cmd.Split(" ");
                    newNode = new DirectoryNode(sizeName[1], false, Convert.ToInt32(sizeName[0]));
                    fromNameToDirectory.Add(sizeName[1], newNode); // Create new node in the dictionary
                    fromNameToDirectory[currentDir].AddChild(sizeName[1]);
                }
            }
        }

        foreach(KeyValuePair<string, DirectoryNode> pair in fromNameToDirectory)
        {
            Console.WriteLine(String.Format("Directory {0} has {1} children", pair.Key, pair.Value.NumberOfChildren()));
            List<string> children = pair.Value.GetChildren();
            foreach(string child in children)
            {
                Console.WriteLine(String.Format("\t{0}", child));
            }
        }
                
                // CurrentDirectoryStack

        // For each command

        // Create directory

        // Change to directory

        // Create file




    }
}