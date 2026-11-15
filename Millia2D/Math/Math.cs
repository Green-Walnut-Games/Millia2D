namespace Millia2d.Mathematics;

public static class Math
{
    public static float Lerp(float first, float second, float time)
    {
        return first * (1 - time) + second * time;
    }

    public static Vector3 Lerp(Vector3 first, Vector3 second, float time)
    {
        float xMath = Lerp(first.X, second.X, time);
        float yMath = Lerp(first.Y, second.Y, time);
        float zMath = Lerp(first.Z, second.Z, time);

        return new(xMath, yMath, zMath);
    }
}
