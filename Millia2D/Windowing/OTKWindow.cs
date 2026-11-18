using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using Millia2D;
using Millia2D.Profiling;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Millia2D.Windowing;

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
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(
            (BlendingFactor)BlendingFactorSrc.SrcAlpha,
            (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha
        );

        // Renderer onLoad here
        MilliaGame.Instance.Renderer.OnLoad();

        SetupImGui();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        ImguiImplOpenGL3.Shutdown();
        ImguiImplOpenTK4.Shutdown();

        // Renderer unload here
        MilliaGame.Instance.Renderer.OnUnload();
    }

    internal static DebugWindow DebugWindow { get; private set; } = new();
    bool isDebugOpen = false;

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
        // Clears the screen using the color set in OnLoad
        GL.Clear(
            ClearBufferMask.ColorBufferBit
                | ClearBufferMask.DepthBufferBit
                | ClearBufferMask.StencilBufferBit
        );

        // Setup ImGui
        ImGuiRenderSetup(args);

        if (KeyboardState.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.F1))
        {
            isDebugOpen = !isDebugOpen;
        }

        double renderFrameTime = Profiler.MeasureTime(
            "Game instance render frame",
            MilliaGame.Instance.Renderer.OnRenderFrame
        );

        Log.LiveMeasurementEngine("Render frame time", renderFrameTime);
        Log.LiveMeasurementEngine(
            "Actors",
            MilliaGame.Instance.LoadedScenes.Sum(x => x.Actors.Count)
        );

        if (isDebugOpen)
        {
            DebugWindow.OnUpdateFrame(args);
        }

        ImGuiPostRenderFrame();

        SwapBuffers();
    }

    private static void ImGuiRenderSetup(FrameEventArgs args)
    {
        ImguiImplOpenGL3.NewFrame();
        ImguiImplOpenTK4.NewFrame();
        ImGui.NewFrame();
        ImGui.DockSpaceOverViewport(
            0,
            ImGui.GetMainViewport(),
            ImGuiDockNodeFlags.PassthruCentralNode
        );
    }

    private void ImGuiPostRenderFrame()
    {
        ImGui.Render();

        ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

        if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
        {
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
            Context.MakeCurrent();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    private void SetupImGui()
    {
        ImGui.CreateContext();
        ImGuiIOPtr io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

        ImGui.StyleColorsDark();

        ImguiImplOpenTK4.Init(this);
        ImguiImplOpenGL3.Init();
    }
}
