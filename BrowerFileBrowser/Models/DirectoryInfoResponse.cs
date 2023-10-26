//Chase Brower, 2023

namespace BrowerFileBrowser.Models;

public class DirectoryInfoResponse
{
    public List<DirectoryInfo> DirectoryInfos { get; set; } = new();

    public string Error { get; set; } = string.Empty;
}
