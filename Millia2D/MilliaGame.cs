namespace Millia2D;

using OpenTK.Windowing.Common;
using Windowing;

public class MilliaGame
{
    // temporarily allow access to _window
    public Window Window { get; private set; }

    public MilliaGame(int width = 800, int height = 600, string title = "Millia2D Game")
    {
        Window = new Window(width, height, title);

        using (Window.otk_Window)
        {
            Window.otk_Window.UpdateFrame += Update;

            Window.otk_Window.Run();
        }
    }

    /// <summary>
    /// Game logic ran every frame.
    /// </summary>
    public virtual void Update(FrameEventArgs eventArgs) { }

    /// <summary>
    /// Game logic ran once at the start of the game.
    /// </summary>
    public virtual void Start() { }
}
