//Chase Brower, 2023

namespace BrowerFileBrowser.Containers;

public class PathStateContainer : StateContainer
{
    public string Path { get; private set; }

    public void SetPath(string path)
    {
        Path = path ?? string.Empty;
        NotifyStateChanged();
    }
}
