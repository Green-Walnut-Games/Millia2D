using Millia2D;
using Millia2D.Mathematics;

namespace tests.Math;

internal class TransformTests
{
    [Test]
    public void LocalPositionAddsOntoPosition()
    {
        Actor parent = new();
        Actor child = new();

        child.Transform.SetParent(parent.Transform);

        parent.Transform.Position = new(5, 5);
        Assert.That(child.Transform.Position, Is.EqualTo(new Vector3(5, 5)));

        child.Transform.LocalPosition = new(1, 1);

        Assert.That(child.Transform.Position, Is.EqualTo(new Vector3(6, 6)));
    }

    [Test]
    public void SettingPositionDirectlyCorrectlySetsLocalPosition()
    {
        Actor parent = new();
        Actor child = new();

        child.Transform.SetParent(parent.Transform);

        parent.Transform.Position = new(5, 5);

        Assert.That(child.Transform.Position, Is.EqualTo(new Vector3(5, 5)));

        child.Transform.Position = new(17, 3);

        Assert.That(child.Transform.Position, Is.EqualTo(new Vector3(17, 3)));

        Assert.That(
            child.Transform.LocalPosition,
            Is.EqualTo(new Vector3(17, 3) - new Vector3(5, 5))
        );
    }
}
