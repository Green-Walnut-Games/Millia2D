using System.Numerics;
using Millia2D;
using Millia2d.Core;
using Millia2d.Rendering;
using Millia2D.Resources;
using OpenTK.Graphics.OpenGL;

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

        Actor cats = new();
        cats.AddComponent(new CatClass());
        Texture catsTexture = new("assets/cats.png");
        Sprite catsSprite = new();
        catsSprite.Texture = catsTexture;
        cats.AddComponent(catsSprite);

        world.AddActor(cats);
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

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
        Actor.Transform.Rotation.Z +=
            10f
            * MathF.Tan((float)Clock.totalElapsedTime)
            * MathF.Sin(2 * (float)Clock.totalElapsedTime)
            * (float)deltaTime;
    }
}

public class CatClass : Component
{
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        Actor.Transform.Position.X = 2 * MathF.Cos((float)Clock.totalElapsedTime);
        Actor.Transform.Position.Y = MathF.Sin((float)Clock.totalElapsedTime * 2);

        Actor.Transform.Scale.X = MathF.Sin((float)Clock.totalElapsedTime);
        Actor.Transform.Scale.Y = MathF.Cos((float)Clock.totalElapsedTime);
    }
}
