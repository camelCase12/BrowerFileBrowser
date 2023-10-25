//Chase Brower, 2023

namespace BrowerFileBrowser.Models;

public class DoubleClickDetector
{
    private object Value;

    private DateTime lastClick = DateTime.MinValue;

    private static readonly TimeSpan doubleClickTime = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Discerns double click if the object passed in is the same as last time and less than 500 milliseconds have passed
    /// </summary>
    /// <param name="value">The thing being interacted with</param>
    /// <returns>Whether it is a double click</returns>
    public bool IsDoubleClick(object value)
    {
        bool isDoubleClick = false;

        if (DateTime.Now - lastClick <= doubleClickTime && Value == value)
        {
            isDoubleClick = true;
        }

        Value = value;
        lastClick = DateTime.Now;

        return isDoubleClick;
    }
}
