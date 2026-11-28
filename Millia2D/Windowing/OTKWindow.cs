using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using Millia2D.Inputs;
using Millia2D.Mathematics;
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

        double renderFrameTime = Profiler.MeasureTime(MilliaGame.Instance.Renderer.OnRenderFrame);
        EngineTelemetry(renderFrameTime);

        double textRenderFrameTime = Profiler.MeasureTime(
            MilliaGame.Instance.UIRenderer.OnRenderFrame
        );
        EngineTelemetry(textRenderFrameTime);

        if (isDebugOpen)
        {
            DebugWindow.OnUpdateFrame(args);
        }

        ImGuiPostRenderFrame();

        SwapBuffers();
    }

    private static void EngineTelemetry(double renderFrameTime)
    {
        Vector3 screenCoordinates = Input.GetMousePosition();
        Log.LiveDataEngine($"Mouse position: ", Input.GetMousePosition());

        Vector3 worldCoordinates = Input.GetWorldCoordinatesFromScreen(screenCoordinates);
        Log.LiveDataEngine($"World Mouse position: ", worldCoordinates);

        Log.LiveMeasurementEngine("Render frame time", renderFrameTime);
        Log.LiveMeasurementEngine(
            "Actors",
            MilliaGame.Instance.LoadedScenes.Sum(x => x.Actors.Count)
        );
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
