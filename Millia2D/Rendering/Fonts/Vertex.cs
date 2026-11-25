using OpenTK.Mathematics;

struct Vertex
{
    public Vector2 Position;
    public Vector2 Texture;
    public uint Color;

    public Vertex(Vector2 pos, Vector2 tex, uint color)
    {
        Position = pos;
        Texture = tex;
        Color = color;
    }
}
