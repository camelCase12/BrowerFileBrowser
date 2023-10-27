//Chase Brower, 2023

using BrowerFileBrowser.Models;

namespace BrowerFileBrowser.Interfaces;

/// <summary>
/// Provides a list of default nodes, unique to each OS or environment
/// </summary>
public interface IDefaultNodeProvider
{
    public HashSet<FileSystemNode> GetDefaultNodes();
}
