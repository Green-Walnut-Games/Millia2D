using Millia2D;

namespace Millia2D.Rendering;

public class Sprite : Component
{
    public Texture Texture { get; set; }

    public override void Preload()
    {
        base.Preload();

        MilliaGame.GameInstance.Renderer.RegisterSprite(this);
    }
}
