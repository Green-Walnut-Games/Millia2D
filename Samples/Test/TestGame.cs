using System.Drawing;
using Millia2D;
using Millia2D.Inputs;
using Millia2D.Logging;
using Millia2D.Mathematics;
using Millia2D.Rendering;
using Millia2D.UI;
using Math = Millia2D.Mathematics.Math;

public class TestGame : MilliaGame
{
    public static Scene world;

    public override void Init()
    {
        Input.DefineAxis("Horizontal", Keys.D, Keys.A);
        Input.DefineAxis("Vertical", Keys.W, Keys.S);

        Log.SetLogLevel(LogLevel.Info);

        LoadFontFromFile("assets/fonts/massia.ttf");

        world = new("World");

        Actor circle = new();
        circle.Transform.Position = new Vector3(2, 2);

        Sprite circleSprite = new();
        Texture circleTexture = new("assets/circle.png");
        circleSprite.Texture = circleTexture;
        circle.AddComponent(circleSprite);
        circle.AddComponent(new Circle());
        circle.Transform.Scale = new Vector3(0.2f, 0.2f, 1);

        Actor labelActor = new();
        labelActor.Transform.Position = new(50, 50);
        Button button = new();
        button.Text = "Hello world!";
        labelActor.AddComponent(button);
        button.FontFace = "MASSIVE 10 A";

        button.Background = true;
        button.BackgroundColor = Color.Black;
        button.BackgroundPadding = 10;
        button.PressedColor = Color.AliceBlue;

        button.OnPress += () => Log.Info("BUTTON HAS BEEN PRESSED");

        circle.GetComponent<Circle>().label = button;

        world.AddActor(circle);
        world.AddActor(labelActor);

        AddScene(world);
    }
}

public class Circle : Component
{
    public Label label;
    float speed = 0.05f;

    Vector3 direction;

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        direction.X = (float)Input.GetAxis("Horizontal");
        direction.Y = (float)Input.GetAxis("Vertical");

        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            Actor circle = new();
            circle.Transform.Position = Input.GetWorldCoordinatesFromScreen(
                Input.GetMousePosition()
            );

            Texture circleTexture = new("assets/circle.png");
            Sprite circleSprite = new();
            circleSprite.Texture = circleTexture;
            circle.AddComponent(circleSprite);
            circle.Transform.Scale = new Vector3(0.2f, 0.2f, 1);

            TestGame.world.AddActor(circle);
        }

        label.Text = $"X: {(int)Transform.Position.X} Y: {(int)Transform.Position.Y}";
    }

    public override void PhysicsUpdate(double deltaTime)
    {
        base.PhysicsUpdate(deltaTime);

        Transform.Position = Math.Lerp(
            Transform.Position,
            Transform.Position + direction * speed,
            1
        );
    }
}
