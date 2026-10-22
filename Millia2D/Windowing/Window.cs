using OpenTK.Windowing.Desktop;

namespace Millia2D.Windowing;

public class Window : GameWindow
{
    public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        
    }
}