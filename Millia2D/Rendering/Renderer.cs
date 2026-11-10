using Millia2D;
using Millia2d.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace Millia2d.Rendering;

public class Renderer
{
    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;
    Shader shader;

    public HashSet<Sprite> Sprites = new();

    public void RegisterSprite(Sprite sprite)
    {
        Console.WriteLine("Registered sprite");
        Sprites.Add(sprite);
    }

    public void OnLoad()
    {
        shader = new("assets/Shaders/shader.vert", "assets/Shaders/shader.frag");
        for (int i = 0; i < Sprites.Count; i++)
        {
            Sprite sprite = Sprites.ElementAt(i);
            LoadSprite(sprite);
        }
    }

    private void LoadSprite(Sprite sprite)
    {
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            sprite.vertices.Length * sizeof(float),
            sprite.vertices,
            BufferUsageHint.StaticDraw
        );

        shader = new("assets/Shaders/shader.vert", "assets/Shaders/shader.frag");

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            sprite.vertices.Length * sizeof(float),
            sprite.vertices,
            BufferUsageHint.StaticDraw
        );

        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            sprite.indices.Length * sizeof(uint),
            sprite.indices,
            BufferUsageHint.StaticDraw
        );

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexArrayAttrib(VertexArrayObject, 0);
    }

    public void OnRenderFrame()
    {
        // Swap the area being buffered with the area being drawn

        GL.UseProgram(0);

        shader.Use();
        GL.BindVertexArray(VertexArrayObject);
        // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        for (int i = 0; i < Sprites.Count; i++)
        {
            Sprite sprite = Sprites.ElementAt(i);
            RenderFrameSprite(sprite);
        }
    }

    private void RenderFrameSprite(Sprite sprite)
    {
        GL.DrawElements(
            PrimitiveType.Triangles,
            sprite.indices.Length,
            DrawElementsType.UnsignedInt,
            0
        );
    }

    public void OnUnload()
    {
        shader.Dispose();
    }
}
