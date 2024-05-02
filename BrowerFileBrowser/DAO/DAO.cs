//Chase Brower, 2023

namespace BrowerFileBrowser.DAO;

public class DAO
{
    public static string DBDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, "BrowerFileBrowser", "Database");
    public static string ConnectionString = $"Data Source={Path.Combine(DBDirectory, "FileTagDatabase.db")}";
    public static string BackupDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, "BrowerFileBrowser", "Backups");
    public static string FileDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, "BrowerFileBrowser", "Files");
}
