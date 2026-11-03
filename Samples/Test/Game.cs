using Millia2D;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Game : MilliaGame
{
    public override void Init()
    {
        Scene world = new("World");

        Actor player = new();
        player.AddComponent(new PlayerClass());

        world.AddActor(player);
    }
}

public class PlayerClass : Component
{
    public override void PhysicsUpdate(double deltaTime)
    {
        Console.WriteLine("Physics update is: " + deltaTime);
    }

    public override void Update(double deltaTime)
    {
        Console.WriteLine("Normal update is: " + deltaTime);
    }
}
