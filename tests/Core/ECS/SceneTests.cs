using Millia2D;

namespace tests.Core.ECS;

public class SceneTests
{
    Scene activeScene;

    [SetUp]
    public void Setup()
    {
        activeScene = new("Testing scene");
    }

    [Test]
    public void AddActorToScene()
    {
        Actor actor = new();

        activeScene.AddActor(actor);

        Assert.That(
            activeScene.Entities.Count,
            Is.EqualTo(1),
            "Should detect an Actor in the list once added"
        );
    }

    [Test]
    public void RemoveActorFromScene()
    {
        Actor Actor = new();

        activeScene.AddActor(Actor);

        Assert.That(activeScene.Entities.Count, Is.EqualTo(1));

        activeScene.RemoveActor(Actor);

        Assert.That(activeScene.Entities.Count, Is.EqualTo(0));
    }

    [Test]
    public void NameAssignedCorrectly()
    {
        Assert.That(activeScene.SceneName, Is.EqualTo("Testing scene"));
    }
}
