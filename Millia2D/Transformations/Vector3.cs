namespace Millia2d.Transformations;

public class Vector3
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

    private static float Lerp(float first, float second, float time)
    {
        return first * (1 - time) + second * time;
    }

    public static Vector3 Lerp(Vector3 first, Vector3 second, float time)
    {
        float xLerp = Lerp(first.X, second.X, time);
        float yLerp = Lerp(first.Y, second.Y, time);
        float zLerp = Lerp(first.Z, second.Z, time);

        return new(xLerp, yLerp, zLerp);
    }

    public static Vector3 operator +(Vector3 left, Vector3 right)
    {
        float totalX = left.X + right.X;
        float totalY = left.Y + right.Y;
        float totalZ = left.Z + right.Z;

        return new(totalX, totalY, totalZ);
    }

    public static Vector3 operator *(Vector3 left, float right)
    {
        return new(left.X * right, left.Y * right, left.Z * right);
    }
}
