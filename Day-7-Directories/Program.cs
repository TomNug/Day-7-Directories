using System;
using System.Collections.Generic;

class DirectoryNode
{
    private string name;
    private bool isFolder;
    private List<string> children;
    private string parent;
    private int size;

    public DirectoryNode(string newName, bool newIsFolder, string newParent, int newSize = 0)
    {
        name = newName;
        isFolder = newIsFolder;
        children = new List<string>();  
        parent = newParent;
        size = newSize;
    }

    public bool IsFolder()
    {
        return isFolder;
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

    public bool HasParent()
    {
        return parent.Length > 0;
    }

    public string GetParent()
    {
        return parent;
    }

    public void AddSize(int additionalSize)
    {
        size += additionalSize;
    }
    public int GetSize()
    {
        return size;
    }
    public string GetName()
    {
        return name;
    }
}

class Program
{
    public static void ParseFileIntoTree(Dictionary<string, DirectoryNode> fromNameToDirectory)
    {
        // Read commands from the file into a string array
        string[] cmds = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day-7-Directories\Day-7-Directories\data_test.txt");

        // Starting in home directory
        string currentDir = "";
        DirectoryNode node = new DirectoryNode("/", true, "");
        fromNameToDirectory.Add("/", node); // Create new node in the dictionary

        // For each command
        foreach (string cmd in cmds)
        {
            // Identify special commands
            if (cmd[0] == '$')
            {
                if (cmd[2] == 'c')
                {
                    // $ cd
                    if (cmd[5] == '.')
                    {
                        // $ cd ..
                        currentDir = fromNameToDirectory[currentDir].GetParent();
                    }
                    else
                    {
                        // $ cd xyz
                        string dir = cmd.Substring(5);
                        currentDir = dir;
                    }
                }
            }
            else // Creating a text file or a directory
            {
                string name;
                // Creating a directory
                if (cmd.Substring(0, 3) == "dir")
                {
                    // Making a new directory
                    name = cmd.Substring(4); // After "dir " to end is the name
                    node = new DirectoryNode(name, true, currentDir);
                    fromNameToDirectory.Add(name, node); // Create new node in the dictionary
                    fromNameToDirectory[currentDir].AddChild(name);
                }
                else // Creating a file
                {
                    string[] sizeName = cmd.Split(" ");
                    int size = Convert.ToInt32(sizeName[0]);
                    name = sizeName[1];
                    node = new DirectoryNode(name, false, currentDir, size);
                    fromNameToDirectory.Add(name, node); // Create new node in the dictionary
                    fromNameToDirectory[currentDir].AddChild(name);

                    // Add the file size to the parents
                    while (node.HasParent())
                    {
                        // Add current file size to the parent
                        string parentName = node.GetParent();
                        fromNameToDirectory[parentName].AddSize(size);

                        // Set current node to parent
                        node = fromNameToDirectory[parentName];

                    }
                }
            }
        }
    }
    public static void Main(string[] args)
    {
        // Create dictionary connecting names of directories/files with their objects
        Dictionary<string, DirectoryNode> fromNameToDirectory = new Dictionary<string, DirectoryNode>();

        // Populate data structure with data from text file
        ParseFileIntoTree(fromNameToDirectory);

        // sum to answer challenge
        int sumUpTo100000 = 0;
        // Check each file/directory
        foreach (KeyValuePair<string, DirectoryNode> pair in fromNameToDirectory)
        {
            Console.WriteLine(String.Format("Directory {0} has {1} children and is {2} bits", pair.Key, pair.Value.NumberOfChildren(), pair.Value.GetSize()));
            if ((pair.Value.IsFolder()) && (pair.Value.GetSize() <= 100000))
            {
                sumUpTo100000 += pair.Value.GetSize();
            }
            List<string> children = pair.Value.GetChildren();
            foreach (string child in children)
            {
                Console.WriteLine(String.Format("\t{0}", child));
            }
        }
        Console.WriteLine(String.Format("The answer to the challenge is {0}", sumUpTo100000));
    }
}