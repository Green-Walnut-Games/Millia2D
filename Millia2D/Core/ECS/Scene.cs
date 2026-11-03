namespace Millia2D;

/// <summary>
/// Represents a level in the game, containing actors and their components.
/// </summary>
public class Scene
{
    /// <summary>
    /// Unique ID.
    /// </summary>
    public Guid GUID { get; private set; }

    /// <summary>
    /// Human-readable scene name to be used in-game.
    /// </summary>
    public string SceneName { get; private set; }

    /// <summary>
    /// List of all actors inside this scene.
    /// </summary>
    public List<Actor> Actors { get; private set; } = new();

    internal bool hasStarted = false;

    public Scene(string sceneName)
    {
        GUID = Guid.NewGuid();
        SceneName = sceneName;
    }

    /// <summary>
    /// Adds an actor to the scene.
    /// </summary>
    /// <param name="actor"></param>
    public void AddActor(Actor actor)
    {
        Actors.Add(actor);
    }

    /// <summary>
    /// Removes an actor from the scene.
    /// </summary>
    /// <param name="actor"></param>
    public void RemoveActor(Actor actor)
    {
        Actors.Remove(actor);
    }

    public override string ToString()
    {
        return $"{GUID}-{SceneName}";
    }

    internal void Preload()
    {
        for (int actor = 0; actor < Actors.Count; actor++)
        {
            for (int comp = 0; comp < Actors[actor].Components.Count; comp++)
            {
                Actors[actor].Components.ElementAt(comp).Preload();
            }
        }
    }

    internal void Load()
    {
        for (int actor = 0; actor < Actors.Count; actor++)
        {
            for (int comp = 0; comp < Actors[actor].Components.Count; comp++)
            {
                Actors[actor].Components.ElementAt(comp).Load();
            }
        }
    }

    internal void PhysicsUpdate(double deltaTime)
    {
        for (int actor = 0; actor < Actors.Count; actor++)
        {
            for (int comp = 0; comp < Actors[actor].Components.Count; comp++)
            {
                Actors[actor].Components.ElementAt(comp).PhysicsUpdate(deltaTime);
            }
        }
    }

    internal void Update(double deltaTime)
    {
        for (int actor = 0; actor < Actors.Count; actor++)
        {
            for (int comp = 0; comp < Actors[actor].Components.Count; comp++)
            {
                Actors[actor].Components.ElementAt(comp).Update(deltaTime);
            }
        }
    }

    internal void LateUpdate(double deltaTime)
    {
        for (int actor = 0; actor < Actors.Count; actor++)
        {
            for (int comp = 0; comp < Actors[actor].Components.Count; comp++)
            {
                Actors[actor].Components.ElementAt(comp).LateUpdate(deltaTime);
            }
        }
    }
}
