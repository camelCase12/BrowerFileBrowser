using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowerFileBrowser.Utils;

public class MouseUtils
{
    private static readonly long leftMouse = 0;
    public static bool IsLeftMouse(MouseEventArgs e)
    {
        return e.Button == leftMouse;
    }
}
