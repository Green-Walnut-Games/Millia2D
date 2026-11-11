using Millia2D;
using Millia2d.Core;
using Millia2d.Rendering;
using Millia2D.Resources;

public class Game : MilliaGame
{
    public override void Init()
    {
        Scene world = new("World");

        Actor player = new();
        player.AddComponent(new PlayerClass());

        Texture texture = new("assets/picture.png");
        Sprite sprite = new();
        sprite.Texture = texture;

        player.AddComponent(sprite);

        player.Transform.Scale.X = 0.2f;

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
        base.Update(deltaTime);
        Actor.Transform.Rotation.Z +=
            1f
            * MathF.Tan((float)Clock.totalElapsedTime)
            * MathF.Sin(2 * (float)Clock.totalElapsedTime);
    }
}
