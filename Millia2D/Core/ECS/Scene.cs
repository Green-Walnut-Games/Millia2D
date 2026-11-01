namespace Millia2D;

/// <summary>
/// Represents a level in the game, containing entities and their components.
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
    /// List of all entities inside this scene.
    /// </summary>
    public List<Actor> Entities { get; private set; } = new();

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
        Entities.Add(actor);
    }

    /// <summary>
    /// Removes an actor from the scene.
    /// </summary>
    /// <param name="actor"></param>
    public void RemoveActor(Actor actor)
    {
        Entities.Remove(actor);
    }

    public override string ToString()
    {
        return $"{GUID}-{SceneName}";
    }
}
