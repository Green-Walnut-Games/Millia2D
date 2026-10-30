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
    public void AddEntityToScene()
    {
        Entity entity = new();

        activeScene.AddEntity(entity);

        Assert.That(
            activeScene.Entities.Count,
            Is.EqualTo(1),
            "Should detect an entity in the list once added"
        );
    }

    [Test]
    public void RemoveEntityFromScene()
    {
        Entity entity = new();

        activeScene.AddEntity(entity);

        Assert.That(activeScene.Entities.Count, Is.EqualTo(1));

        activeScene.RemoveEntity(entity);

        Assert.That(activeScene.Entities.Count, Is.EqualTo(0));
    }

    [Test]
    public void NameAssignedCorrectly()
    {
        Assert.That(activeScene.SceneName, Is.EqualTo("Testing scene"));
    }
}
