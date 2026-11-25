using System.Drawing;

namespace Millia2D.UI;

public class Button : Label
{
    public Action OnPress;

    public Color HoverColor { get; set; } = Color.Gray;

    internal bool isHovered = false;

    internal void Press()
    {
        OnPress.Invoke();
    }
}
