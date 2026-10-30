using Millia2D;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Game : MilliaGame
{
    public override void Start()
    {
        // Example: new scene and player entity
        Scene mainScene = new();
        Entity player = new();

        mainScene.AddEntity(player);
    }

    public override void Update(FrameEventArgs eventArgs)
    {
        // Temporary until input story is played
        if (Window.IsKeyPressed(Keys.Escape))
        {
            Window.Close();
        }
    }
}
