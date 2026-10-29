namespace Millia2D;

using OpenTK.Windowing.Common;
using Windowing;

public class MilliaGame
{
    protected Window _window;

    public MilliaGame(int width = 800, int height = 600, string title = "Millia2D Game")
    {
        _window = new Window(width, height, title);

        using (_window)
        {
            _window.UpdateFrame += Update;

            _window.Run();
        }
    }

    public void CloseWindow()
    {
        _window.Dispose();
        _window.Close();
    }

    /// <summary>
    /// Game logic ran every frame.
    /// </summary>
    public virtual void Update(FrameEventArgs eventArgs) { }
}
