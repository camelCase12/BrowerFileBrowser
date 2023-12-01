//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;

[assembly: Dependency(typeof(ThumbnailService))]
namespace BrowerFileBrowser.Interfaces;

public class ThumbnailService : IThumbnailService
{
    public async Task<string> GetVideoThumbnailURLAsync(string videoFilePath, float maxPixelLength = 150)
    {
        throw new NotImplementedException();
    }
}