using Millia2D;
using Millia2d.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Vector3 = Millia2d.Transformations.Vector3;

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
        Shader activeShader;
        // Picks correct shader for sprite
        if (sprite.Texture != null)
        {
            activeShader = TextureShader;
        }
        else
        {
            activeShader = ColorShader;
        }
        activeShader.Use();

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
            ApplyTextureVAO(sprite);
        }
    }

    private void ApplyTextureVAO(Sprite sprite)
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

    public void OnRenderFrame()
    {
        for (int i = 0; i < Sprites.Count; i++)
        {
            RenderFrameSprite(Sprites.ElementAt(i));
        }
    }

    private void RenderFrameSprite(Sprite sprite)
    {
        Shader activeShader;
        // Picks correct shader for sprite
        if (sprite.Texture != null)
        {
            activeShader = TextureShader;
        }
        else
        {
            activeShader = ColorShader;
        }
        activeShader.Use();

        ApplyTransformations(sprite, activeShader);

        GL.DrawElements(
            PrimitiveType.Triangles,
            sprite.indices.Length,
            DrawElementsType.UnsignedInt,
            0
        );
    }

    private static void ApplyTransformations(Sprite sprite, Shader activeShader)
    {
        float gameHeight = 10;

        float windowHeight = MilliaGame.GameInstance.Window.Height;
        float windowWidth = MilliaGame.GameInstance.Window.Width;

        float aspect = windowWidth / windowHeight;
        float gameWidth = aspect * gameHeight;

        Matrix4 orthographicProjection = Matrix4.CreateOrthographic(
            gameWidth,
            gameHeight,
            -500f,
            500f
        );

        Matrix4 view = Matrix4.CreateTranslation(0, 0, 0);

        Vector3 spriteRotation = sprite.Actor.Transform.Rotation;
        Matrix4 rotationZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(spriteRotation.Z));
        Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spriteRotation.Y));
        Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spriteRotation.X));

        Vector3 spriteScale = sprite.Actor.Transform.Scale;
        Matrix4 scale = Matrix4.CreateScale(spriteScale.X, spriteScale.Y, spriteScale.Z);

        Vector3 spritePosition = sprite.Actor.Transform.Position;
        Matrix4 position = Matrix4.CreateTranslation(
            spritePosition.X,
            spritePosition.Y,
            spritePosition.Z
        );

        Matrix4 finalTrans = scale * rotationZ * position * view * orthographicProjection;

        int transformLocation = GL.GetUniformLocation(activeShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref finalTrans);
    }

    public void OnUnload()
    {
        ColorShader.Dispose();
        TextureShader.Dispose();
    }
}
