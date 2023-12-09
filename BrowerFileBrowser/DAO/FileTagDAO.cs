//Chase Brower, 2023

using Microsoft.Data.Sqlite;

namespace BrowerFileBrowser.DAO;

public static class FileTagDAO
{
    public static void AddFile(string filePath)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string insertFileQuery = "INSERT OR IGNORE INTO Files (FilePath) VALUES (@FilePath);";
        using var command = new SqliteCommand(insertFileQuery, connection);
        command.Parameters.AddWithValue("@FilePath", filePath);
        command.ExecuteNonQuery();
    }

    public static void AddTag(string tagName)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string insertTagQuery = "INSERT OR IGNORE INTO Tags (TagName) VALUES (@TagName);";
        using var command = new SqliteCommand(insertTagQuery, connection);
        command.Parameters.AddWithValue("@TagName", tagName);
        command.ExecuteNonQuery();
    }

    public static void MapFileToTag(string filePath, string tagName)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string mappingQuery = @"
            INSERT OR IGNORE INTO FileTagMapping (FileID, TagID)
            SELECT f.FileID, t.TagID FROM Files f, Tags t
            WHERE f.FilePath = @FilePath AND t.TagName = @TagName;";

        using var command = new SqliteCommand(mappingQuery, connection);
        command.Parameters.AddWithValue("@FilePath", filePath);
        command.Parameters.AddWithValue("@TagName", tagName);
        command.ExecuteNonQuery();
    }

    public static void UnmapFileFromTag(string filePath, string tagName)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        // Unmapping the file from the tag
        string unmappingQuery = @"
        DELETE FROM FileTagMapping
        WHERE FileID IN (SELECT FileID FROM Files WHERE FilePath = @FilePath)
        AND TagID IN (SELECT TagID FROM Tags WHERE TagName = @TagName);";

        using (var command = new SqliteCommand(unmappingQuery, connection))
        {
            command.Parameters.AddWithValue("@FilePath", filePath);
            command.Parameters.AddWithValue("@TagName", tagName);
            command.ExecuteNonQuery();
        }

        // Check if there are no more mappings for this tag
        string checkMappingQuery = @"
        SELECT COUNT(*)
        FROM FileTagMapping
        WHERE TagID IN (SELECT TagID FROM Tags WHERE TagName = @TagName);";

        using (var command = new SqliteCommand(checkMappingQuery, connection))
        {
            command.Parameters.AddWithValue("@TagName", tagName);
            int mappingCount = Convert.ToInt32(command.ExecuteScalar());

            // If there are no mappings left, delete the tag
            if (mappingCount == 0)
            {
                string deleteTagQuery = @"
                DELETE FROM Tags
                WHERE TagName = @TagName;";

                using (var deleteCommand = new SqliteCommand(deleteTagQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@TagName", tagName);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }
    }


    public static List<string> GetTagsForFile(string filePath)
    {
        List<string> tags = new List<string>();
        string connectionString = "Data Source=FileTagDatabase.sqlite;Version=3;";
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string query = @"
        SELECT t.TagName FROM Tags t
        JOIN FileTagMapping ftm ON t.TagID = ftm.TagID
        JOIN Files f ON ftm.FileID = f.FileID
        WHERE f.FilePath = @FilePath;";

        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@FilePath", filePath);
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            tags.Add(reader["TagName"].ToString());
        }
        return tags;
    }

    public static List<string> GetFilesForTag(string tagName)
    {
        List<string> files = new List<string>();
        using var connection = new SqliteConnection(DAO.ConnectionString);
        
        connection.Open();

        string query = @"
        SELECT f.FilePath FROM Files f
        JOIN FileTagMapping ftm ON f.FileID = ftm.FileID
        JOIN Tags t ON ftm.TagID = t.TagID
        WHERE t.TagName = @TagName;";

        using var command = new SqliteCommand(query, connection);
            
        command.Parameters.AddWithValue("@TagName", tagName);
        using SqliteDataReader reader = command.ExecuteReader();
                
        while (reader.Read())
        {
            files.Add(reader["FilePath"].ToString());
        }
        return files;
    }

    public static string GetFirstFileForTag(string tagName)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string query = @"
        SELECT f.FilePath FROM Files f
        JOIN FileTagMapping ftm ON f.FileID = ftm.FileID
        JOIN Tags t ON ftm.TagID = t.TagID
        WHERE t.TagName = @TagName
        LIMIT 1;";

        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@TagName", tagName);
        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return reader["FilePath"].ToString();
        }

        return null; // Return null if no file is found
    }


    public static List<string> GetTags()
    {
        List<string> tags = new List<string>();
        using var connection = new SqliteConnection(DAO.ConnectionString);
        
        connection.Open();

        string query = "SELECT TagName FROM Tags;";

        using var command = new SqliteCommand(query, connection);

        using SqliteDataReader reader = command.ExecuteReader();
                
        while (reader.Read())
        {
            tags.Add(reader["TagName"].ToString());
        }
        return tags;
    }

}
