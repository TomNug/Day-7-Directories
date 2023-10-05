using System;
using System.Collections.Generic;
using System.Text; // For the string builder
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
        //string[] cmds = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day-7-Directories\Day-7-Directories\data_test.txt");
        string[] cmds = System.IO.File.ReadAllLines(@"C:\Users\Tom\Documents\Advent\Day-7-Directories\Day-7-Directories\data_full.txt");
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

                        // Special case for first command
                        if (dir == "/")
                        {
                            currentDir = dir;
                        }
                        // Change to a directory
                        else
                        {
                            // Use the format currentDirectory/newDirectory
                            currentDir = currentDir + "/" + dir;
                        }
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
                    // New node params
                    // DirectoryName, and the parent is currentDir
                    node = new DirectoryNode(name, true, currentDir);
                    // Determine directory's full name
                    string nameAndPath = currentDir + "/" + name;
                    // The full name is needed in the dictionary
                    fromNameToDirectory.Add(nameAndPath, node); // Create new node in the dictionary
                    // Current directory's full name is used in order to add the child 
                    fromNameToDirectory[currentDir].AddChild(name);
                }

                else // Creating a file
                {
                    string[] sizeName = cmd.Split(" ");
                    int size = Convert.ToInt32(sizeName[0]);
                    name = sizeName[1];
                    node = new DirectoryNode(name, false, currentDir, size);
                    // Current directory is full name
                    // Child name is shortened
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

        int totalUsedMemory = fromNameToDirectory["/"].GetSize();
        int requiredFreeSpace = 30000000;
        int diskSize = 70000000;
        int diskUsed = diskSize - totalUsedMemory;
        int minimumDirSize = requiredFreeSpace - diskUsed;

        Console.WriteLine(String.Format("Using {0} of {1}", totalUsedMemory, diskSize));
        Console.WriteLine(String.Format("Need to delete {0}", requiredFreeSpace));

        // Need to delete one directory
        // The smallest directory with at least minimumDirSize will be deleted to free up enough space
        int sizeOfDirectoryToDelete = diskSize;

        // Check each file/directory
        foreach (KeyValuePair<string, DirectoryNode> pair in fromNameToDirectory)
        {
            // If folder, big enough, and smaller than previous
            if ((pair.Value.IsFolder()) && 
                (pair.Value.GetSize() >= minimumDirSize) && 
                (pair.Value.GetSize() < sizeOfDirectoryToDelete))
            {
                Console.WriteLine(String.Format("{0} needed. New best option: {1}", minimumDirSize, pair.Value.GetSize()));
                sizeOfDirectoryToDelete = pair.Value.GetSize();
            }
        }
        Console.WriteLine(String.Format("The folder to delete has a size of {0}", sizeOfDirectoryToDelete));
    }
}