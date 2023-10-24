//Chase Brower, 2023

namespace BrowerFileBrowser.Interfaces;

public interface IFileViewer
{
    List<FileInfo> Files { get; set; }
    List<DirectoryInfo> Directories { get; set; }
}
