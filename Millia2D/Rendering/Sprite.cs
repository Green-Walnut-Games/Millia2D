namespace Millia2D.Rendering;

public class Sprite : Component
{
    public Texture Texture { get; set; }

    public override void Preload()
    {
        base.Preload();

        MilliaGame.Instance.Renderer.RegisterSprite(this);
    }
}
