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

    public static Vector3 operator -(Vector3 left)
    {
        return new(-left.X, -left.Y, -left.Z);
    }

    public static Vector3 operator *(Vector3 left, float right)
    {
        return new(left.X * right, left.Y * right, left.Z * right);
    }

    public static Vector3 operator /(Vector3 left, float right)
    {
        return new(left.X / right, left.Y / right, left.Z / right);
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

    /// <summary>
    /// Return the distance between two coordinates.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static double Distance(Vector3 target, Vector3 origin)
    {
        // sqrt (x^2 + y^2 + z^2)
        Vector3 difference = target - origin;

        return Math.Sqrt(
            difference.X * difference.X + difference.Y * difference.Y + difference.Z * difference.Z
        );
    }

    /// <summary>
    /// Returns the dot product between two vectors.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static double DotProduct(Vector3 target, Vector3 origin)
    {
        return target.X * origin.X + target.Y * origin.Y + target.Z * origin.Z;
    }

    /// <summary>
    /// Returns the length of the vector.
    /// </summary>
    /// <returns></returns>
    public double Magnitude()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Returns the angle in radians between the two vectors.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static double AngleBetween(Vector3 target, Vector3 origin)
    {
        // double dotProduct = DotProduct(target, origin);
        // double magnitudeTarget = target.Magnitude();
        // double magnitudeOrigin = origin.Magnitude();
        // double result = Math.Acos(dotProduct / (magnitudeOrigin * magnitudeTarget));

        // if (Double.IsNaN(result))
        //     result = 0;

        // return result;

        return Math.Atan2(target.Y - origin.Y, target.X - origin.X);
    }

    public Vector3 Max(float maxValue)
    {
        return new(Math.Min(X, maxValue), Math.Min(Y, maxValue), Math.Min(Z, maxValue));
    }

    public bool IsNaN()
    {
        return !float.IsNaN(X) && !float.IsNaN(Y) && !float.IsNaN(Z);
    }
}
