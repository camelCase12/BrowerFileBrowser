//Chase Brower, 2023

using BrowerFileBrowser.Interfaces;
using BrowerFileBrowser.Models;
using System.Runtime.InteropServices;

namespace BrowerFileBrowser.Factory;

public static class DefaultNodeProviderFactory
{
    public static IDefaultNodeProvider Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsDefaultNodeProvider();
        }

        throw new PlatformNotSupportedException();
    }
}
