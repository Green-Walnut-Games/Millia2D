namespace Millia2D;

using Millia2d.Core;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using Windowing;

public class MilliaGame
{
    public static MilliaGame? GameInstance { get; private set; }

    // temporarily allow access to _window
    public Window Window { get; private set; }

    public List<Scene> LoadedScenes { get; private set; } = new();

    public MilliaGame(int width = 800, int height = 600, string title = "Millia2D Game")
    {
        GameInstance = this;

        Window = new Window(width, height, title);

        using (Window.otk_Window)
        {
            Init();

            Preload();
            Load();

            Window.otk_Window.UpdateFrame += UpdateFrame;

            Window.otk_Window.Run();
        }
    }

    public virtual void Init() { }

    public void AddScene(Scene scene)
    {
        if (!scene.hasStarted)
        {
            scene.Preload();
            scene.Load();
        }
        LoadedScenes.Add(scene);
    }

    public void RemoveScene(Scene scene)
    {
        LoadedScenes.Remove(scene);
    }

    internal void Preload()
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            if (LoadedScenes[i].hasStarted)
                continue;
            LoadedScenes[i].Preload();
        }
    }

    internal void Load()
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            if (LoadedScenes[i].hasStarted)
                continue;
            LoadedScenes[i].Load();
        }
    }

    internal void PhysicsUpdate(double deltaTime)
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            LoadedScenes[i].PhysicsUpdate(deltaTime);
        }
    }

    internal void Update(double deltaTime)
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            LoadedScenes[i].Update(deltaTime);
        }
    }

    internal void LateUpdate(double deltaTime)
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            LoadedScenes[i].LateUpdate(deltaTime);
        }
    }

    double lastPhysicsTime = 0;

    internal void UpdateFrame(FrameEventArgs eventArgs)
    {
        // Time elapsed since last frame is eventArgs.Time

        Clock.Update(eventArgs.Time);

        // Handle physics update
        // If more than physics update time has elapsed, we need to run a physics update
        if (Clock.totalElapsedTime >= lastPhysicsTime + Clock.PhysicsUpdateTime)
        {
            double physicsDeltaTime = Clock.totalElapsedTime - lastPhysicsTime;

            // If time elapsed is more than the physics update time, we update by the difference.
            // This means that if a long time passes between updating physics, when we do get to updating it,
            // we update it by however much time had passed.
            PhysicsUpdate(physicsDeltaTime);

            lastPhysicsTime = Clock.totalElapsedTime;
        }

        Update(eventArgs.Time);
        LateUpdate(eventArgs.Time);
    }
}
