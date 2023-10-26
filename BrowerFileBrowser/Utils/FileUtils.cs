//Chase Brower, 2023

using BrowerFileBrowser.Models;

namespace BrowerFileBrowser.Utils;

public class FileUtils
{
    public static bool TryGetDirectories(string path, out DirectoryInfoResponse directoryInfoResponse)
    {
        List<DirectoryInfo> directories = new();
        try
        {
            directories.AddRange(Directory.GetDirectories(path).Select(directory => new DirectoryInfo(directory)));
        }
        catch (Exception ex)
        {
            directoryInfoResponse = new() { DirectoryInfos = directories, Error = ex.Message };
            return false;
        }
        directoryInfoResponse = new() { DirectoryInfos = directories };
        return true;
    }
        
    public static bool TryGetFiles(string path, out FileInfoResponse fileInfoResponse)
    {
        List<FileInfo> files = new();
        try
        {
            files.AddRange(Directory.GetFiles(path).Select(file => new FileInfo(file)));
        }
        catch (Exception ex)
        {
            fileInfoResponse = new() { FileInfos = files, Error = ex.Message };
            return false;
        }
        fileInfoResponse = new() { FileInfos = files };
        return true;
    }
}
