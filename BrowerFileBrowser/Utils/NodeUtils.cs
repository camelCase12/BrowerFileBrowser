using BrowerFileBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowerFileBrowser.Utils;

public class NodeUtils
{
    public static void AddSpecialNode(string path, string altName, HashSet<FileSystemNode> nodeList, bool pinnable = true)
    {
        FileSystemNode? node = GetSpecialNode(path, altName, pinnable);
        if (node != null)
        {
            nodeList.Add(node);
        }
    }

    public static FileSystemNode? GetSpecialNode(string path, string altName, bool pinnable = true)
    {
        try
        {
            FileSystemNode node = new FileSystemNode()
            {
                FileSystemInfo = new DirectoryInfo(path),
                AltName = altName,
                Pinnable = pinnable,
            };

            return node;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
