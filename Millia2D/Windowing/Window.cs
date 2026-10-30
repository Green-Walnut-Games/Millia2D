using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Millia2D.Windowing;

public class Window
{
    internal GameWindow otk_Window;

    public Window(int width, int height, string title)
    {
        otk_Window = new(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        );
    }

    public bool IsKeyPressed(Keys key)
    {
        return otk_Window.KeyboardState.IsKeyDown(key);
    }

    public void Close()
    {
        otk_Window.Dispose();
        otk_Window.Close();
    }
}
