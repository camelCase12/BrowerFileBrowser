//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;
using SkiaSharp;

namespace BrowerFileBrowser.Utils;

public static class ImageUtils
{
    private static readonly List<string> supportedImageExtensions = new()
    {
        ".png", ".jpg", ".gif", ".bmp", ".webp", ".ico", ".tiff", ".wbmp", ".pkm"
    };

    private static readonly List<string> supportedVectorExtensions = new()
    {
        ".svg"
    };

    private static readonly List<string> supportedVideoExtensions = new()
    {
        ".webm", ".mp4", "wmv", ".avi", ".m4v", ".mov", ".3gp"
    };

    public static async Task<string> GetThumbnailAsDataURL(IThumbnailService thumbnailService, FileInfo fileInfo, float maxPixelLength = 150)
    {
        if (supportedImageExtensions.Contains(fileInfo.Extension))
        {
            return GetImageAsDataURL(fileInfo);
        }

        else if (supportedVectorExtensions.Contains(fileInfo.Extension))
        {
            return GetVectorAsDataURL(fileInfo);
        }

        else if (supportedVideoExtensions.Contains(fileInfo.Extension))
        {
            return await GetVideoThumbnailAsDataURLAsync(thumbnailService, fileInfo.FullName, maxPixelLength);
        }

        return string.Empty;
    }

    public static string GetVectorAsDataURL(FileInfo fileInfo)
    {
        string vectorData = File.ReadAllText(fileInfo.FullName);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(vectorData);
        string base64 = Convert.ToBase64String(bytes);

        return $"data:image/svg+xml;base64,{base64}";
    }

    public static string GetImageAsDataURL(FileInfo fileInfo, float thumbnailWidth = 150, float thumbnailHeight = 150)
    {
        using var inputStream = fileInfo.OpenRead();
        using var original = SKBitmap.Decode(inputStream);

        float scale = Math.Min(thumbnailWidth / original.Width, thumbnailHeight / original.Height);
        int newWidth = (int)(original.Width * scale);
        int newHeight = (int)(original.Height * scale);
        using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);

        using var image = SKImage.FromBitmap(resized);
        using var output = new MemoryStream();

        // Encode the image to the specified stream as JPEG
        image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(output);
        byte[] imageBytes = output.ToArray();

        // Convert the byte array to a Base64 string
        string base64String = Convert.ToBase64String(imageBytes);
        return "data:image/jpeg;base64," + base64String;
    }

    public static async Task<string> GetVideoThumbnailAsDataURLAsync(IThumbnailService thumbnailService, string videoFilePath, float maxPixelLength)
    {
        // Use the IThumbnailService to get the thumbnail stream
        string base64String = await thumbnailService.GetVideoThumbnailBase64Async(videoFilePath, maxPixelLength);

        return "data:image/jpeg;base64," + base64String;
    }


    public static bool IsSupportedSkiaSharpImage(string extension)
    {
        return supportedImageExtensions.Contains(extension);
    }
}