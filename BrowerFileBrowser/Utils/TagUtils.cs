//Chase Brower, 2023

using BrowerFileBrowser.DAO;

namespace BrowerFileBrowser.Utils;

public static class TagUtils
{
    public static List<FileInfo> GetFiles(string tagURI)
    {
        if (!tagURI.StartsWith("Tags:/")) return new();

        string tagCsv = tagURI.Substring(tagURI.IndexOf("/")+1);

        List<string> tags = tagCsv.Split(",").ToList();
        List<string> filePaths = new();

        foreach(string tag in tags)
        {
            filePaths.AddRange(FileTagDAO.GetFilesForTag(tag));
        }

        List<FileInfo> files = filePaths.Select(x => new FileInfo(x)).ToList();

        return files;
    }
}