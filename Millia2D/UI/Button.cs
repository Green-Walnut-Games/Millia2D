namespace Millia2D.UI;

public class Button : Label
{
    public Action OnPress;

    internal void Press()
    {
        OnPress.Invoke();
    }
}
