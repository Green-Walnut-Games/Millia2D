using Millia2D;
using OpenTK.Windowing.Common;

public class Game : MilliaGame
{
    public override void Update(FrameEventArgs eventArgs)
    {
        if (_window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
        {
            CloseWindow();
        }
    }
}
