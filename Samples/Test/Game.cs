using Millia2D;
using Millia2d.Rendering;
using Millia2D.Resources;

public class Game : MilliaGame
{
    public override void Init()
    {
        Scene world = new("World");

        Actor player = new();
        player.AddComponent(new PlayerClass());
        Sprite sprite = new();
        player.AddComponent(sprite);

        world.AddActor(player);
        AddScene(world);
    }
}

public class PlayerClass : Component
{
    public override void Preload()
    {
        base.Preload();
        Console.WriteLine("Player preload");
    }

    public override void PhysicsUpdate(double deltaTime)
    {
        Console.WriteLine("Physics update is: " + deltaTime);
    }

    public override void Update(double deltaTime)
    {
        Console.WriteLine("Normal update is: " + deltaTime);
    }
}
