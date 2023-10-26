//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;

namespace BrowerFileBrowser.Models;

public class FileInfoResponse
{
    public List<FileInfo> FileInfos { get; set; } = new();

    public string Error { get; set; } = string.Empty;
}
