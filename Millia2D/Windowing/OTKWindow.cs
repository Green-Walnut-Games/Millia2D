using Millia2d.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Millia2d.Windowing;

internal class OTKWindow : GameWindow
{
    public OTKWindow(
        GameWindowSettings gameWindowSettings,
        NativeWindowSettings nativeWindowSettings
    )
        : base(gameWindowSettings, nativeWindowSettings) { }

    float[] vertices =
    {
        -0.5f,
        -0.5f,
        0.0f, //Bottom-left vertex
        0.5f,
        -0.5f,
        0.0f, //Bottom-right vertex
        0.0f,
        0.5f,
        0.0f, //Top vertex
    };

    int VertexBufferObject;
    int VertexArrayObject;
    Shader shader;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 0.5f, 1.0f);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw
        );

        shader = new("assets/Shaders/shader.vert", "assets/Shaders/shader.frag");

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw
        );

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexArrayAttrib(VertexArrayObject, 0);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        shader.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        // Clears the screen using the color set in OnLoad
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Swap the area being buffered with the area being drawn

        GL.UseProgram(0);

        shader.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
