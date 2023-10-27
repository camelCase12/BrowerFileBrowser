//Chase Brower, 2023

namespace BrowerFileBrowser.Containers;

public class PathStateContainer : StateContainer
{
    //TODO maybe configurable?
    private readonly static int MAX_PATH_HISTORY = 150;

    public List<string> Paths { get; private set; } = new();

    public string Path { get; private set; } = string.Empty;

    public bool AtFront => depth == 0;

    public bool AtBack => depth >= Paths.Count - 1;

    private int depth = 0;

    public void SetPath(string? path)
    {
        Path = path ?? string.Empty;

        ResetDepth();

        AddPath();

        NotifyStateChanged();
    }

    private void ResetDepth()
    {
        Paths.RemoveRange(Paths.Count - depth, depth);
        depth = 0;
    }

    private void AddPath()
    {
        if (Paths.Count == MAX_PATH_HISTORY) Paths.RemoveAt(0);

        Paths.Add(Path);
    }

    public bool NavigateBackward()
    {
        if (AtBack) return false;

        depth++;

        Path = Paths[Paths.Count - 1 - depth];

        NotifyStateChanged();
        return true;
    }

    public bool NavigateForward()
    {
        if (AtFront) return false;

        depth--;

        Path = Paths[Paths.Count - 1 - depth];

        NotifyStateChanged();
        return true;
    }
}
