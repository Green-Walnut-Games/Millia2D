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

    public Component()
    {
        GUID = Guid.NewGuid();
    }
}
