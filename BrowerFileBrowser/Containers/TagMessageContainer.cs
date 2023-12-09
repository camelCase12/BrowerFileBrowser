//Chase Brower, 2023

namespace BrowerFileBrowser.Containers;

/// <summary>
/// Message container for those interested in when tags change
/// </summary>
public class TagMessageContainer : StateContainer
{
    private int MAX_TAGS = 4;

    public void TagStateChanged() => NotifyStateChanged();

    public List<string> RecentTags { get; set; } = new();

    public void AddRecentTag(string tag)
    {
        if(RecentTags.Count == MAX_TAGS)
        {
            RecentTags.RemoveAt(0);
        }
        if(RecentTags.Contains(tag))
        {
            RecentTags.Remove(tag);
        }
        RecentTags.Add(tag);
    }
}