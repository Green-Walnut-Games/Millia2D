namespace Millia2D.Mathematics;

// TODO: Do we need a separate transform for UI components?
public class Transform : Component
{
    public Vector3 Position { get; set; } = new();
    public Vector3 Scale { get; set; } = new(1, 1, 1);
    public Vector3 Rotation { get; set; } = new(0, 0, 90);
}
