

namespace BrowerFileBrowser.Utils;

public class FileUtils
{
    public static List<DirectoryInfo> GetDirectories(string path) =>
        Directory.GetDirectories(path).Select(directory => new DirectoryInfo(directory)).ToList();

    public static List<FileInfo> GetFiles(string path) =>
        Directory.GetFiles(path).Select(file => new FileInfo(file)).ToList();
}
