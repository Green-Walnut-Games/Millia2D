using Millia2d.Windowing;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Millia2D.Windowing;

public class Window
{
    internal OTKWindow otk_Window;

    public float Width => otk_Window.Size.X;
    public float Height => otk_Window.Size.Y;

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
