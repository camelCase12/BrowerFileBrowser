//Chase Brower, 2023

using Microsoft.Data.Sqlite;

namespace BrowerFileBrowser.DAO;

public static class DatabaseInitialization
{
    public static void InitializeDatabase()
    {
        Directory.CreateDirectory(DAO.DBDirectory);

        string dbFilePath = Path.Combine(DAO.DBDirectory, "FileTagDatabase.db");

        using var connection = new SqliteConnection($"Data Source={dbFilePath}");
        connection.Open();

        string createFilesTable = "CREATE TABLE IF NOT EXISTS Files (FileID INTEGER PRIMARY KEY AUTOINCREMENT, FilePath TEXT UNIQUE);";
        string createTagsTable = "CREATE TABLE IF NOT EXISTS Tags (TagID INTEGER PRIMARY KEY AUTOINCREMENT, TagName TEXT UNIQUE);";
        string createMappingTable = "CREATE TABLE IF NOT EXISTS FileTagMapping (FileID INTEGER, TagID INTEGER, PRIMARY KEY(FileID, TagID), FOREIGN KEY(FileID) REFERENCES Files(FileID), FOREIGN KEY(TagID) REFERENCES Tags(TagID));";

        using var command = new SqliteCommand(createFilesTable + createTagsTable + createMappingTable, connection);
        command.ExecuteNonQuery();
    }
}
