using Millia2D;

namespace Millia2d.Rendering;

public class Sprite : Component
{
    public float[] vertices =
    {
        0.5f,
        0.5f,
        0.0f,
        1.0f,
        1.0f, // top right
        0.5f,
        -0.5f,
        0.0f,
        1.0f,
        0.0f, // bottom right
        -0.5f,
        -0.5f,
        0.0f,
        0.0f,
        0.0f, // bottom left
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        1.0f, // top left
    };

    public uint[] indices =
    {
        0,
        1,
        3, // first triangle
        1,
        2,
        3, // second triangle
    };

    public Texture Texture { get; set; }

    public override void Preload()
    {
        base.Preload();

        Console.WriteLine("Sprite preload");
        MilliaGame.GameInstance.Renderer.RegisterSprite(this);
    }
}
