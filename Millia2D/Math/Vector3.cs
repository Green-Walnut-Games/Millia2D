namespace Millia2D.Mathematics;

public struct Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(float x, float y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    public static Vector3 Zero()
    {
        return new Vector3();
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
        float totalX = left.X + right.X;
        float totalY = left.Y + right.Y;
        float totalZ = left.Z + right.Z;

        return new(totalX, totalY, totalZ);
    }

    public static Vector3 operator -(Vector3 left, Vector3 right)
    {
        float totalX = left.X - right.X;
        float totalY = left.Y - right.Y;
        float totalZ = left.Z - right.Z;

        return new(totalX, totalY, totalZ);
    }

    public static Vector3 operator *(Vector3 left, float right)
    {
        return new(left.X * right, left.Y * right, left.Z * right);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Vector3 objVector = (Vector3)obj;

        return Equals(objVector);
    }

    public bool Equals(Vector3 vec)
    {
        return vec.X == X && vec.Y == Y && vec.Z == Z;
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}
