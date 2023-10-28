//Chase Brower, 2023

using BrowerFileBrowser.Utils;

namespace BrowerFileBrowser.Models;

public class FileSystemNode
{
    public FileSystemInfo? FileSystemInfo { get; set; }
    public HashSet<FileSystemNode> Children { get; set; } = new();
    public string AltName { get; set; } = string.Empty;
    public string PreferredName => string.IsNullOrEmpty(AltName) ? FileSystemInfo?.Name ?? string.Empty : AltName;
    public bool Pinnable = true;

    public HashSet<FileSystemNode> GetChildren()
    {
        if (Children.Count > 0) return Children;

        if (!FileUtils.TryGetDirectories(FileSystemInfo?.FullName, out DirectoryInfoResponse response))
        {
            return new();
        }

        HashSet<FileSystemNode> children = new();

        foreach(var child in response.DirectoryInfos)
        {
            NodeUtils.AddSpecialNode(child.FullName, string.Empty, children, false);
        }

        return children;
    }
}
