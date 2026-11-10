using Millia2D;
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

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 0.5f, 1.0f);

        // Renderer onLoad here
        MilliaGame.GameInstance.Renderer.OnLoad();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        // Renderer unload here
        MilliaGame.GameInstance.Renderer.OnUnload();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        // Clears the screen using the color set in OnLoad
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Renderer onRenderFrame here
        MilliaGame.GameInstance.Renderer.OnRenderFrame();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
