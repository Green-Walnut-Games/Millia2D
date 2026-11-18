using Millia2d;
using Millia2D;
using Millia2d.Logging;
using Millia2d.Mathematics;
using Millia2d.Rendering;
using Math = Millia2d.Mathematics.Math;

public class Game : MilliaGame
{
    public static Scene world;

    public override void Init()
    {
        Log.SetLogLevel(LogLevel.Info);
        world = new("World");

        Actor spawnerActor = new();
        Spawner spawner = new();
        spawnerActor.AddComponent(spawner);

        world.AddActor(spawnerActor);

        AddScene(world);
    }
}

public class Spawner : Component
{
    public override void PhysicsUpdate(double deltaTime)
    {
        base.PhysicsUpdate(deltaTime);

        Actor circle = new();

        Texture circleTexture = new("assets/circle.png");
        Sprite circleSprite = new();
        circleSprite.Texture = circleTexture;
        circle.AddComponent(circleSprite);
        circle.AddComponent(new Circle());
        circle.Transform.Scale = new Vector3(0.2f, 0.2f, 1);

        Game.world.AddActor(circle);
    }
}

public class Circle : Component
{
    Random rnd = new();

    float initX,
        initY;

    float speed = 0.001f;

    Vector3 direction;

    public override void Preload()
    {
        base.Preload();
        initX = (float)rnd.NextDouble() * 20 - 10;
        initY = (float)rnd.NextDouble() * 10 - 5;

        direction = new(initX, initY, 0);

        // Randomize position
        Actor.Transform.Position = new Vector3(initX, initY, 0);
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        if (rnd.NextDouble() > 0.8)
        {
            direction = new((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1);
        }

        Actor.Transform.Position = Math.Lerp(
            Actor.Transform.Position,
            Actor.Transform.Position + direction * speed,
            5
        );
    }
}
