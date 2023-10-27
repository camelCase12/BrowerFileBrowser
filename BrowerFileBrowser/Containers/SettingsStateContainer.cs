//Chase Brower, 2023

using BrowerFileBrowser.Enums;

namespace BrowerFileBrowser.Containers;

public class SettingsStateContainer : StateContainer
{
    public FileViewerType SelectedFileViewerType { get; private set; }

    public void SetFileViewerType(FileViewerType? selectedFileViewerType)
    {
        SelectedFileViewerType = selectedFileViewerType ?? default;
        NotifyStateChanged();
    }
}
