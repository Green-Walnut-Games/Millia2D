using System.Reflection.Metadata;

namespace Millia2D;

/// <summary>
/// Component class - attached to an object, it provides functionality and logic.
/// </summary>
public class Component
{
    /// <summary>
    /// Unique ID.
    /// </summary>
    public Guid GUID { get; private set; }

    public Actor? Actor { get; internal set; }

    public Component()
    {
        GUID = Guid.NewGuid();
    }

    public virtual void Preload() { }

    public virtual void Load() { }

    public virtual void PhysicsUpdate(double deltaTime) { }

    public virtual void Update(double deltaTime) { }

    public virtual void LateUpdate(double deltaTime) { }
}
