namespace Millia2D;

/// <summary>
/// Represents a level in the game, containing entities and their components.
/// </summary>
public class Scene
{
    public List<Entity> Entities { get; private set; } = new();

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
}
