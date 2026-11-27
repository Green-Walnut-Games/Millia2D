namespace Millia2D.Mathematics;

// TODO: Do we need a separate transform for UI components?
public class Transform : Component
{
    public Transform Parent { get; private set; } = null;
    public List<Transform> Children { get; private set; } = new();
    internal Vector3 _Position = new();
    public Vector3 Position
    {
        get
        {
            if (Parent == null)
            {
                return _Position;
            }
            else
            {
                return Parent.Position + LocalPosition;
            }
        }
        set
        {
            if (Parent == null)
            {
                _Position = value;
            }
            else
            {
                _Position = value;
                _LocalPosition = value - Parent.Position;
            }
        }
    }
    internal Vector3 _LocalPosition = new();
    public Vector3 LocalPosition
    {
        get { return _LocalPosition; }
        set
        {
            if (Parent == null)
            {
                _Position = value;
                _LocalPosition = value;
            }
            else
            {
                _LocalPosition = value;
                _Position = Parent.Position + _LocalPosition;
            }
        }
    }
    public Vector3 Scale { get; set; } = new(1, 1, 1);
    public Vector3 Rotation { get; set; } = new(0, 0, 90);

    public void SetParent(Transform newParent)
    {
        Parent = newParent;
    }

    public void AddChild(Transform child)
    {
        Vector3 difference = child.Position - Position;

        Children.Add(child);
        child.Parent = this;

        child.LocalPosition = difference;
    }

    public void RemoveChild(Transform child)
    {
        Vector3 finalPosition = child.Parent.Position + child.LocalPosition;

        Children.Remove(child);
        child.Parent = null;

        child.Position = finalPosition;
    }

    public Transform GetChild(int index)
    {
        return Children.ElementAt(index);
    }
}
