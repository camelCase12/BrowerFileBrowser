//Chase Brower, 2023

using SkiaSharp;

namespace BrowerFileBrowser.Utils;

public static class ImageUtils
{
    private static readonly List<string> supportedImageExtensions = new()
    {
        ".png", ".jpg", ".gif", ".bmp", ".webp", ".ico", ".tiff", ".wbmp", ".pkm" 
    };

    public static string GetImageAsDataURL(FileInfo fileInfo)
    {
        using var inputStream = fileInfo.OpenRead();
        using var original = SKBitmap.Decode(inputStream);

        using var image = SKImage.FromBitmap(original);
        using var output = new MemoryStream();

        // Encode the image to the specified stream as JPEG
        image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(output);
        byte[] imageBytes = output.ToArray();

        // Convert the byte array to a Base64 string
        string base64String = Convert.ToBase64String(imageBytes);
        return "data:image/jpeg;base64," + base64String;
    }

    public static string GetThumbnailAsDataURL(FileInfo fileInfo, float thumbnailWidth = 150, float thumbnailHeight = 150)
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

    public static bool IsSupportedSkiaSharpImage(string extension)
    {
        return supportedImageExtensions.Contains(extension);
    }
}