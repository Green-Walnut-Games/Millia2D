namespace Millia2D;

using Windowing;

public class MilliaGame
{
    private Window _window;

    public MilliaGame(int width = 800, int height = 600, string title = "Millia2D Game")
    {
        _window = new Window(width, height, title);

        using (_window)
        {
            _window.Run();
        }
    }
}
