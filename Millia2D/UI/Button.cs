using System.Drawing;

namespace Millia2D.UI;

public class Button : Label
{
    public Action OnPress;

    public Color HoverColor { get; set; } = Color.Gray;
    public Color PressedColor { get; set; } = Color.DarkSlateGray;

    public bool IsHovered { get; internal set; } = false;
    public bool IsPressed { get; internal set; } = false;

    internal void Press()
    {
        OnPress.Invoke();
    }
}
