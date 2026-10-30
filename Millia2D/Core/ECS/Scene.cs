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
    public List<Entity> Entities { get; private set; } = new();

    public Scene(string sceneName)
    {
        GUID = Guid.NewGuid();
        SceneName = sceneName;
    }

    /// <summary>
    /// Adds an entity to the scene.
    /// </summary>
    /// <param name="entity"></param>
    public void AddEntity(Entity entity)
    {
        Entities.Add(entity);
    }

    /// <summary>
    /// Removes an entity from the scene.
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveEntity(Entity entity)
    {
        Entities.Remove(entity);
    }

    public override string ToString()
    {
        return $"{GUID}-{SceneName}";
    }
}
