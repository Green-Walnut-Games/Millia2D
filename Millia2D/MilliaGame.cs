namespace Millia2D;

using Windowing;

public class MilliaGame
{
    private Window _window;

    public MilliaGame()
    {
        _window = new Window(800, 600, "Millia2D Game");

        using (_window)
        {
            _window.Run();
        }
    }
}
