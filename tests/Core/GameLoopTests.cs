using Millia2D;
using Millia2d.Core;
using OpenTK.Windowing.Common;

namespace tests.Core;

public class GameLoopTests
{
    [Test]
    public void UpdateRunsCorrectAmountOfTimes()
    {
        MilliaGame game = new();

        Scene scene = new("Test scene");
        Actor player = new();
        player.AddComponent(new LoopCounter());
        scene.AddActor(player);
        game.AddScene(scene);

        LoopCounter loop = player.GetComponent<LoopCounter>();

        Assert.That(Clock.totalElapsedTime, Is.EqualTo(0));
        Assert.That(loop.preloadCount, Is.EqualTo(1));
        Assert.That(loop.loadCount, Is.EqualTo(1));

        game.UpdateFrame(new FrameEventArgs(0.1));
        Assert.That(Clock.totalElapsedTime, Is.EqualTo(0.1));

        Assert.That(loop.preloadCount, Is.EqualTo(1));
        Assert.That(loop.loadCount, Is.EqualTo(1));
        // Physics update is 1 here because we perform a physics update right at the start of the game.
        Assert.That(loop.physicsUpdateCount, Is.EqualTo(1));
        Assert.That(loop.updateCount, Is.EqualTo(1));
        Assert.That(loop.lateUpdateCount, Is.EqualTo(1));

        game.UpdateFrame(new FrameEventArgs(0.2));
        Assert.That(Clock.totalElapsedTime - 0.3, Is.LessThan(0.001));

        Assert.That(loop.preloadCount, Is.EqualTo(1));
        Assert.That(loop.loadCount, Is.EqualTo(1));
        Assert.That(loop.physicsUpdateCount, Is.EqualTo(2));
        Assert.That(loop.updateCount, Is.EqualTo(2));
        Assert.That(loop.lateUpdateCount, Is.EqualTo(2));
    }
}

public class LoopCounter : Component
{
    public int preloadCount;
    public int loadCount;
    public int physicsUpdateCount;
    public int updateCount;
    public int lateUpdateCount;

    public override void Preload()
    {
        preloadCount++;
    }

    public override void Load()
    {
        loadCount++;
    }

    public override void PhysicsUpdate(double deltaTime)
    {
        physicsUpdateCount++;
    }

    public override void Update(double deltaTime)
    {
        updateCount++;
    }

    public override void LateUpdate(double deltaTime)
    {
        lateUpdateCount++;
    }
}
