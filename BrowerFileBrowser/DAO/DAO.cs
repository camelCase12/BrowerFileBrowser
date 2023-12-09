//Chase Brower, 2023

namespace BrowerFileBrowser.DAO;

public class DAO
{
    public static string DBDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, "BrowerFileBrowser", "Database");
    public static string ConnectionString = $"Data Source={Path.Combine(DBDirectory, "FileTagDatabase.db")}";
}
