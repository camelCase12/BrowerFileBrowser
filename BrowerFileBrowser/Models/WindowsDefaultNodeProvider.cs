//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;
using BrowerFileBrowser.Utils;

namespace BrowerFileBrowser.Models;

public class WindowsDefaultNodeProvider : IDefaultNodeProvider
{
    public HashSet<FileSystemNode> GetDefaultNodes()
    {
        HashSet<FileSystemNode> defaultNodes = new ();

        // Add special folders
        NodeUtils.AddSpecialNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", defaultNodes);
        NodeUtils.AddSpecialNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads", "Downloads", defaultNodes);
        NodeUtils.AddSpecialNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", defaultNodes);
        NodeUtils.AddSpecialNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "User", defaultNodes);

        // Add drives
        FileSystemNode thisPCNode = new FileSystemNode() { Children = GetDrives(), AltName = "This PC", Pinnable = false };
        defaultNodes.Add(thisPCNode);

        return defaultNodes;
    }

    private HashSet<FileSystemNode> GetDrives()
    {
        try
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            HashSet<FileSystemNode> drives = new();

            foreach (DriveInfo d in allDrives)
            {
                drives.Add(new() { FileSystemInfo = new DirectoryInfo(d.Name), Pinnable = false } );
            }

            return drives;
        }
        catch(Exception)
        {
            return new();
        }
    }
}
