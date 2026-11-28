using Millia2D.Mathematics;

namespace tests.Math;

internal class VectorTests
{
    [Test]
    public void DotProduct()
    {
        Vector3 v1 = new(2, 5, 0);
        Vector3 v2 = new(4, -3, 0);

        Assert.That(Vector3.DotProduct(v1, v2), Is.EqualTo(-7));
    }

    [Test]
    public void MagnitudeUnit()
    {
        Vector3 v1 = new(1, 0, 0);
        Assert.That(v1.Magnitude(), Is.EqualTo(1));
    }

    [Test]
    public void MagnitudeSquare()
    {
        Vector3 v1 = new(3, 4, 0);
        Assert.That(v1.Magnitude(), Is.EqualTo(5));
    }

    [Test]
    public void AngleBetweenSelfAndUp()
    {
        Vector3 up = new(0, 1, 0);
        Vector3 self = new(0, 2, 0);
        Assert.That(Vector3.AngleBetween(up, self), Is.EqualTo(0));
    }

    [Test]
    public void AngleBetweenSelfAndRight()
    {
        Vector3 up = new(0, 0, 0);
        Vector3 self = new(2, 0, 0);
        Assert.That(Vector3.AngleBetween(up, self), Is.EqualTo(0));
    }
}
