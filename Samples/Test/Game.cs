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

        Texture texture = new("assets/picture.png");
        Sprite sprite = new();
        sprite.Texture = texture;

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
}
