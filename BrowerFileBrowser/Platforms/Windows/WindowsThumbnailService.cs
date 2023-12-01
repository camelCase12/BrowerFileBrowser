//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;
using Windows.Media.Editing;
using Windows.Storage.Streams;
using Windows.Storage;
using SkiaSharp;

[assembly: Dependency(typeof(ThumbnailService))]
namespace BrowerFileBrowser.Interfaces;

public class ThumbnailService : IThumbnailService
{
    public async Task<string> GetVideoThumbnailBase64Async(string videoFilePath, float maxPixelLength = 150)
    {
        var file = await StorageFile.GetFileFromPathAsync(videoFilePath);
        var clip = await MediaClip.CreateFromFileAsync(file);
        var composition = new MediaComposition();
        composition.Clips.Add(clip);

        TimeSpan timeFromStart = TimeSpan.FromSeconds(1); // or any other specific time
        VideoFramePrecision framePrecision = VideoFramePrecision.NearestFrame;

        var videoProperties = await file.Properties.GetVideoPropertiesAsync();
        double aspectRatio = (double)videoProperties.Width / videoProperties.Height;

        float scale = Math.Min(maxPixelLength / videoProperties.Width, maxPixelLength / videoProperties.Height);
        int newWidth = (int)(videoProperties.Width * scale);
        int newHeight = (int)(videoProperties.Height * scale);

        using var thumbnailStream = await composition.GetThumbnailAsync(timeFromStart, (int)newWidth, (int)newHeight, framePrecision);

        var outputStream = new InMemoryRandomAccessStream();
        await RandomAccessStream.CopyAsync(thumbnailStream, outputStream);

        outputStream.Seek(0);

        using var skiaStream = outputStream.AsStream();
        using var original = SKBitmap.Decode(skiaStream);

        using var image = SKImage.FromBitmap(original);
        using var output = new MemoryStream();

        image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(output);
        byte[] imageBytes = output.ToArray();
        string base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }
}