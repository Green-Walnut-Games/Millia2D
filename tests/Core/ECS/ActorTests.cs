using Millia2D;

namespace tests.Core.ECS;

public class ActorTests
{
    [Test]
    public void ActorGetComponent()
    {
        Actor player = new();
        Component newComp = new();

        player.AddComponent(newComp);

        Assert.That(player.GetComponent<Component>(), Is.EqualTo(newComp));
    }

    [Test]
    public void ActorGetComponents()
    {
        Actor player = new();
        Component newComp = new();
        Component newComp2 = new();

        player.AddComponent(newComp);
        player.AddComponent(newComp2);

        Assert.That(player.GetComponents<Component>().Count, Is.EqualTo(2));
    }

    [Test]
    public void ActorGetCustomComponents()
    {
        Actor player = new();
        TestClass1 newComp = new();
        TestClass2 newComp2 = new();

        player.AddComponent(newComp);
        player.AddComponent(newComp2);

        Assert.That(player.GetComponents<TestClass1>().Count, Is.EqualTo(1));
        Assert.That(player.GetComponents<TestClass2>().Count, Is.EqualTo(1));
    }

    [Test]
    public void ActorRemoveComponent()
    {
        Actor player = new();
        Component newComp = new();

        player.AddComponent(newComp);

        Assert.That(player.Components.Count, Is.EqualTo(1));

        player.RemoveComponent(newComp);

        Assert.That(player.Components.Count, Is.EqualTo(0));
    }
}

internal class TestClass1 : Component { }

internal class TestClass2 : Component { }
