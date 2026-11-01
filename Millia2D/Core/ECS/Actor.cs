using System.ComponentModel;

namespace Millia2D;

/// <summary>
/// Actor class - represents an in-game object.
/// </summary>
public class Actor
{
    /// <summary>
    /// Unique ID.
    /// </summary>
    public Guid GUID { get; private set; }

    /// <summary>
    /// Human-readable actor name to be used in-game.
    /// </summary>
    public string ActorName { get; set; }

    /// <summary>
    /// Hashset of components attached to this actor.
    /// </summary>
    public HashSet<Component> Components { get; private set; } = new();

    public Actor(string actorName = "New Actor")
    {
        GUID = Guid.NewGuid();
        ActorName = actorName;
    }

    /// <summary>
    /// Attach a component to this actor.
    /// </summary>
    /// <param name="component"></param>
    public void AddComponent(Component component)
    {
        Components.Add(component);
    }

    /// <summary>
    /// Detach a component from this actor.
    /// </summary>
    /// <param name="component"></param>
    public void RemoveComponent(Component component)
    {
        Components.Remove(component);
    }

    /// <summary>
    /// Return the first component of the type specified as the generic type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComponent<T>()
        where T : Component
    {
        return Components.OfType<T>().First();
    }

    /// <summary>
    /// Return all components that match the type generic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<Component> GetComponents<T>()
        where T : Component
    {
        return Components.OfType<T>();
    }
}
