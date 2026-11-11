using Millia2D;
using Millia2d.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace Millia2d.Rendering;

public class Renderer
{
    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;
    Shader ColorShader;
    Shader TextureShader;

    public HashSet<Sprite> Sprites = new();

    public void RegisterSprite(Sprite sprite)
    {
        Console.WriteLine("Registered sprite");
        Sprites.Add(sprite);
    }

    public void OnLoad()
    {
        ColorShader = new("assets/Shaders/color-shader.vert", "assets/Shaders/color-shader.frag");
        TextureShader = new(
            "assets/Shaders/texture-shader.vert",
            "assets/Shaders/texture-shader.frag"
        );

        for (int i = 0; i < Sprites.Count; i++)
        {
            Sprite sprite = Sprites.ElementAt(i);
            LoadSprite(sprite);
        }
    }

    private void LoadSprite(Sprite sprite)
    {
        // Picks correct shader for sprite
        if (sprite.Texture != null)
        {
            TextureShader.Use();
        }
        else
        {
            ColorShader.Use();
        }

        VertexBufferObject = GL.GenBuffer();
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

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        if (sprite.Texture != null)
        {
            int texture0Location = TextureShader.GetUniformLocation("texture0");
            GL.Uniform1(texture0Location, 0);

            GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Handle);
            int texCoordLocation = TextureShader.GetAttribLocation("aTexCoord");

            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(
                texCoordLocation,
                2,
                VertexAttribPointerType.Float,
                false,
                5 * sizeof(float),
                3 * sizeof(float)
            );
        }
    }

    public void OnRenderFrame()
    {
        for (int i = 0; i < Sprites.Count; i++)
        {
            RenderFrameSprite(Sprites.ElementAt(i));
        }
    }

    private void RenderFrameSprite(Sprite sprite)
    {
        if (sprite.Texture != null)
        {
            TextureShader.Use();
        }
        else
        {
            ColorShader.Use();
        }

        GL.DrawElements(
            PrimitiveType.Triangles,
            sprite.indices.Length,
            DrawElementsType.UnsignedInt,
            0
        );
    }

    public void OnUnload()
    {
        ColorShader.Dispose();
        TextureShader.Dispose();
    }
}
