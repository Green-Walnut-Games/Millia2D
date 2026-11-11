using Millia2D;

namespace Millia2d.Rendering;

public class Sprite : Component
{
    public Texture Texture { get; set; }

    public override void Preload()
    {
        base.Preload();

        Console.WriteLine("Sprite preload");
        MilliaGame.GameInstance.Renderer.RegisterSprite(this);
    }
}
