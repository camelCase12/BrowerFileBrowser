//Chase Brower, 2023

namespace BrowerFileBrowser.Interfaces;

public interface IThumbnailService
{
    Task<string> GetVideoThumbnailBase64Async(string videoPath, float maxPixelLength);
}