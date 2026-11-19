using Millia2D;
using Millia2D;
using Millia2D.Core;
using Millia2D.Input;
using Millia2D.Logging;
using Millia2D.Mathematics;
using Millia2D.Rendering;
using Math = Millia2D.Mathematics.Math;

public class TestGame : MilliaGame
{
    public static Scene world;

    public override void Init()
    {
        Input.DefineAxis("Horizontal", Keys.D, Keys.A);
        Input.DefineAxis("Vertical", Keys.W, Keys.S);

        Log.SetLogLevel(LogLevel.Info);
        world = new("World");

        Actor circle = new();
        circle.Transform.Position = new Vector3(2, 2);

        Log.Info(circle.Transform.Position.ToString());

        Texture circleTexture = new("assets/circle.png");
        Sprite circleSprite = new();
        circleSprite.Texture = circleTexture;
        circle.AddComponent(circleSprite);
        circle.AddComponent(new Circle());
        circle.Transform.Scale = new Vector3(0.2f, 0.2f, 1);

        world.AddActor(circle);

        AddScene(world);
    }
}

public class Circle : Component
{
    float speed = 0.05f;

    Vector3 direction;

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        direction.X = (float)Input.GetAxis("Horizontal");
        direction.Y = (float)Input.GetAxis("Vertical");

        if (Input.IsMouseButtonDown(MouseButton.Left))
        {
            Log.Info(Input.GetScreenCoordinatesFromWorld(new Vector3(0, 0)).ToString());
            Log.Info(Input.GetWorldCoordinatesFromScreen(new Vector3(1920, 0)).ToString());
            Actor circle = new();
            circle.Transform.Position = Input.GetWorldCoordinatesFromScreen(
                Input.GetMousePosition()
            );

            Log.Info(circle.Transform.Position.ToString());

            Texture circleTexture = new("assets/circle.png");
            Sprite circleSprite = new();
            circleSprite.Texture = circleTexture;
            circle.AddComponent(circleSprite);
            circle.Transform.Scale = new Vector3(0.2f, 0.2f, 1);

            TestGame.world.AddActor(circle);
        }
    }

    public override void PhysicsUpdate(double deltaTime)
    {
        base.PhysicsUpdate(deltaTime);

        Transform.Position = Math.Lerp(
            Transform.Position,
            Transform.Position + direction * speed,
            1
        );

        Log.LiveData($"Mouse position X: ", Input.GetMousePosition().X);
        Log.LiveData($"Mouse position Y: ", Input.GetMousePosition().Y);
    }
}
