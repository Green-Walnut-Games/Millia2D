namespace Millia2d.Core;

public static class Clock
{
    /// <summary>
    /// Total elapsed time since the start of the game.
    /// </summary>
    public static double totalElapsedTime;

    /// <summary>
    /// Time scale of the game loop. 0 would pause the game, 2 would speed up the game 2x.
    /// </summary>
    public static double timeScale;

    /// <summary>
    /// Time designated for physics updates.
    /// eg. default: 0.2 means physics updates run at 50Hz, whilst framerate can go up or down.
    /// </summary>
    public static double PhysicsUpdateTime { get; set; } = 0.02;

    /// <summary>
    /// Responsible for incrementing global time counter with the frame time.
    /// </summary>
    /// <param name="deltaTime"></param>
    public static void IncrementTimer(double deltaTime)
    {
        totalElapsedTime += deltaTime;
    }
}
