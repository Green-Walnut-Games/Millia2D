using System.Numerics;
using Millia2D;
using Millia2D.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Vector3 = Millia2D.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;

namespace Millia2D.Rendering;

internal class Renderer
{
    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;
    Shader ColorShader;
    Shader TextureShader;
    internal HashSet<Sprite> Sprites = new();

    float[] vertices =
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

    uint[] indices =
    {
        0,
        1,
        3, // first triangle
        1,
        2,
        3, // second triangle
    };

    internal void RegisterSprite(Sprite sprite)
    {
        Sprites.Add(sprite);
    }

    internal void OnLoad()
    {
        ColorShader = new("assets/Shaders/color-shader.vert", "assets/Shaders/color-shader.frag");
        TextureShader = new(
            "assets/Shaders/texture-shader.vert",
            "assets/Shaders/texture-shader.frag"
        );

        VertexBufferObject = GL.GenBuffer();
        VertexArrayObject = GL.GenVertexArray();
        ElementBufferObject = GL.GenBuffer();

        SetupVAOs();
    }

    private void SetupVAOs()
    {
        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw
        );

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            indices.Length * sizeof(uint),
            indices,
            BufferUsageHint.StaticDraw
        );

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        ApplyTextureVAO();
    }

    private void ApplyTextureVAO()
    {
        int texture0Location = TextureShader.GetUniformLocation("texture0");
        GL.Uniform1(texture0Location, 0);

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

    static Matrix4 orthographicProjection;
    static Matrix4 view;
    static float windowWidth;
    static float windowHeight;

    internal void OnRenderFrame()
    {
        float gameHeight = 10;

        // Use client size instead of window height in order to get true accurate size of playable area
        windowHeight = MilliaGame.Instance.Window.otk_Window.ClientSize.Y;
        windowWidth = MilliaGame.Instance.Window.otk_Window.ClientSize.X;

        float aspect = windowWidth / windowHeight;
        float gameWidth = aspect * gameHeight;

        orthographicProjection = Matrix4.CreateOrthographic(gameWidth, gameHeight, -500f, 500f);
        view = Matrix4.CreateTranslation(0, 0, 0);

        GL.BindVertexArray(VertexArrayObject);
        for (int i = 0; i < Sprites.Count; i++)
        {
            Sprite sprite = Sprites.ElementAt(i);

            RenderFrameSprite(sprite);
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

        if (sprite.Texture != null)
            GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Handle);

        ApplyTransformations(sprite, activeShader);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    private static void ApplyTransformations(Sprite sprite, Shader activeShader)
    {
        Vector3 spriteRotation = sprite.Transform.Rotation;
        Matrix4 rotationZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(spriteRotation.Z));
        Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spriteRotation.Y));
        Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spriteRotation.X));

        Vector3 spriteScale = sprite.Transform.Scale;
        Matrix4 scale = Matrix4.CreateScale(spriteScale.X, spriteScale.Y, spriteScale.Z);

        Vector3 spritePosition = sprite.Transform.Position;
        Matrix4 position = Matrix4.CreateTranslation(
            spritePosition.X,
            spritePosition.Y,
            spritePosition.Z
        );

        Matrix4 finalTrans = scale * rotationZ * position * view * orthographicProjection;

        int transformLocation = GL.GetUniformLocation(activeShader.Handle, "transform");
        GL.UniformMatrix4(transformLocation, true, ref finalTrans);
    }

    internal static Vector3 GetScreenCoordinatesFromWorld(Vector3 coords)
    {
        Matrix4 resultMatrix = view * orthographicProjection;

        Vector4 result = Vector4.TransformRow(
            new Vector4(coords.X, coords.Y, coords.Z, 1),
            resultMatrix
        );

        // scale result by the window size. Right now, result is in NDC.
        // NDC is -1 to 1, whereas screen would be 0 to width
        // + 1, / 2, * width

        float x = (result.X + 1) / 2 * windowWidth;
        float y = (result.Y + 1) / 2 * windowHeight;

        return new Vector3(x, windowHeight - y, 0);
    }

    internal static Vector3 GetWorldCoordinatesFromScreen(Vector3 coords)
    {
        float NDCX = coords.X * 2 / windowWidth - 1;
        float NDCY = coords.Y * 2 / windowHeight - 1;

        Matrix4 resultMatrix = view * orthographicProjection;

        Vector4 inverseResult = Vector4.TransformRow(
            new Vector4(NDCX, NDCY, coords.Z, 1),
            resultMatrix.Inverted()
        );

        return new Vector3(inverseResult.X, -inverseResult.Y, 0);
    }

    internal void OnUnload()
    {
        ColorShader.Dispose();
        TextureShader.Dispose();
    }
}
