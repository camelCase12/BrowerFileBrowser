//Chase Brower, 2023

using BrowerFileBrowser.Models;
using Microsoft.Data.Sqlite;
using System.IO.Compression;

namespace BrowerFileBrowser.DAO;

public static class FileTagDAO
{
    public static void ChangeFilePath(string oldPath, string newPath)
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string updateFilePathQuery = "UPDATE Files SET FilePath = @NewPath WHERE FilePath = @OldPath;";
        using var command = new SqliteCommand(updateFilePathQuery, connection);
        command.Parameters.AddWithValue("@NewPath", newPath);
        command.Parameters.AddWithValue("@OldPath", oldPath);
        command.ExecuteNonQuery();
    }

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

    public static List<string> GetFilesPaginated(int page, int pageSize)
    {
        List<string> files = new List<string>();
        using var connection = new SqliteConnection(DAO.ConnectionString);

        connection.Open();

        string query = @"
        SELECT FilePath FROM Files
        LIMIT @PageSize OFFSET @Offset";

        using var command = new SqliteCommand(query, connection);

        int offset = (page) * pageSize;

        command.Parameters.AddWithValue("@PageSize", pageSize);
        command.Parameters.AddWithValue("@Offset", offset);
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

    public static int GetFileCount()
    {
        using var connection = new SqliteConnection(DAO.ConnectionString);
        connection.Open();

        string query = @"SELECT COUNT(*) FROM Files";

        using var command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return reader.GetInt32(0);
        }

        return 0;
    }

    public static async Task<bool> CreateBackupFromTags(IProgress<int> progress)
    {
        string backupDirectory = DAO.BackupDirectory;
        string backupName = $"browserFileBrowserBackup-{DateTime.Now:yyyyMMddHHmmss}.bak";
        string backupLocation = Path.Combine(backupDirectory, backupName);

        var databaseFile = Path.Combine(DAO.DBDirectory, "FileTagDatabase.db");

        // Prepare the backup directory and check available space
        Directory.CreateDirectory(backupDirectory); // Ensures the backup directory exists

        SqliteConnection.ClearAllPools();

        // Backup the database file first
        await BackupFileToZip(databaseFile, backupLocation);

        int totalFiles = GetFileCount();
        int processedFiles = 0;

        int PAGE_SIZE = 100;

        int totalPages = totalFiles / PAGE_SIZE + 1;

        for (int i = 0; i < totalPages; i++)
        {
            var files = GetFilesPaginated(i, PAGE_SIZE);

            foreach (var file in files)
            {
                await BackupFileToZip(file, backupLocation);
                processedFiles++;
                progress.Report((processedFiles * 100) / totalFiles);
            }
        }

        return true;
    }

    private static async Task BackupFileToZip(string sourceFilePath, string zipFilePath)
    {
        using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.OpenOrCreate))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                ZipArchiveEntry zipEntry = archive.CreateEntryFromFile(sourceFilePath, sourceFilePath);
                
                using (var fileStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                using (var entryStream = zipEntry.Open())
                {
                    await fileStream.CopyToAsync(entryStream);
                }
            }
        }
    }

    public static async Task RestoreBackupAsync(Stream zipFile, IProgress<int> progress)
    {
        string destinationDirectory = DAO.FileDirectory;
        Directory.CreateDirectory(destinationDirectory);  // Ensure the destination directory exists
        
        using (Stream proxyStream = new AsyncProxyStream(zipFile))  // Wrap the async stream with the proxy
        {
            using (ZipArchive archive = new ZipArchive(proxyStream, ZipArchiveMode.Read))
            {
                // Calculate the total size of all entries for progress calculation
                int totalSize = (int)archive.Entries.Sum(entry => entry.Length);
                int totalRead = 0;

                var databaseEntry = archive.Entries
                    .Where(entry => Path.GetFileName(entry.FullName) == "FileTagDatabase.db")
                    .First();

                string dbLocation = Path.Combine(DAO.DBDirectory, databaseEntry.FullName);

                if (File.Exists(dbLocation))
                {
                    SqliteConnection.ClearAllPools();
                    File.Delete(dbLocation);
                }

                // Extract database file asynchronously using streams
                using (Stream originalFileStream = databaseEntry.Open())
                using (FileStream decompressedFileStream = new FileStream(dbLocation, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, true))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = await originalFileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        await decompressedFileStream.WriteAsync(buffer, 0, bytesRead);
                        totalRead += bytesRead;
                        progress.Report((totalRead * 100) / totalSize);
                    }
                }


                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if(Path.GetFileName(entry.FullName) == "FileTagDatabase.db")
                    {
                        continue;
                    }

                    string destinationPath = Path.Combine(destinationDirectory, Path.GetFileName(entry.FullName));

                    // Handle file name conflicts
                    destinationPath = GetUniqueFilePath(destinationPath);

                    //TODO change file name
                    ChangeFilePath(entry.FullName, destinationPath);

                    // Extract file asynchronously using streams
                    using (Stream originalFileStream = entry.Open())
                    using (FileStream decompressedFileStream = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = await originalFileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            await decompressedFileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;
                            progress.Report((totalRead * 100) / totalSize);
                        }
                    }
                }
            }
        }
    }

    private static string GetUniqueFilePath(string path)
    {
        if (!File.Exists(path))
            return path;

        string directory = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        int number = 1;

        do
        {
            string newFileName = $"{fileName} ({number++}){extension}";
            path = Path.Combine(directory, newFileName);
        } while (File.Exists(path));

        return path;
    }
}
