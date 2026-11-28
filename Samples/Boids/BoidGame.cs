using Millia2D;
using Millia2D.Inputs;
using Millia2D.Mathematics;
using Millia2D.Rendering;

public class BoidGame : MilliaGame
{
    public HashSet<Transform> Boids = new();

    public override void Init()
    {
        Scene mainScene = new("World");
        Window.SetHeight(100);

        Random rnd = new();

        // Set up boids here.
        int numberOfBoids = 300;

        for (int i = 0; i < numberOfBoids; i++)
        {
            Actor boidActor = new();
            Boid boid = new(ref Boids);
            boidActor.AddComponent(boid);
            boid.id = i;

            boidActor.Transform.Position += new Vector3(
                ((float)rnd.NextDouble() - 0.5f) * 15,
                ((float)rnd.NextDouble() - 0.5f) * 15
            );

            Sprite boidSprite = new();
            Texture texture = new("assets/ship.png");
            boidSprite.Texture = texture;
            boidActor.AddComponent(boidSprite);

            mainScene.AddActor(boidActor);
            Boids.Add(boidActor.Transform);
        }

        AddScene(mainScene);
    }
}

/// <summary>
/// Each boid will be an individual in the flocking algorithm, and will be responsible for its own logic.
/// </summary>
public class Boid : Component
{
    public int id;
    HashSet<Transform> allBoids = new();

    public Vector3 velocity { get; private set; } = new();
    public float separationRange = 2f;
    public float cohesionRange = 5f;
    public float alignmentRange = 7f;

    Vector3 separation = new();
    float separationStrength = 100f;
    Vector3 cohesion = new();
    float cohesionStrength = 7.5f;
    Vector3 alignment = new();
    float alignmentStrength = 200f;
    Vector3 toMouse = new();
    float toMouseStrength = 10f;
    float maxValue = 0.3f;

    // Screen borders
    float topEdge;
    float bottomEdge;
    float leftEdge;
    float rightEdge;

    public Boid(ref HashSet<Transform> boids)
    {
        allBoids = boids;
    }

    public override void PhysicsUpdate(double deltaTime)
    {
        SetScreenEdges();

        Separation();
        Cohesion();
        Alignment();

        Vector3 mouseWorldPos = Input.GetWorldCoordinatesFromScreen(Input.GetMousePosition());
        if (!float.IsNaN(mouseWorldPos.X) && Input.IsMouseButtonDown(MouseButton.Left))
        {
            if ((mouseWorldPos - Transform.Position).Magnitude() < 20)
                toMouse = mouseWorldPos - Transform.Position;
        }
        else
        {
            toMouse = Vector3.Zero();
        }

        Vector3 finalVelocity =
            separation * separationStrength
            + cohesion * cohesionStrength
            + alignment * alignmentStrength
            + toMouse * toMouseStrength;
        velocity += finalVelocity * (float)deltaTime * 0.01f;
        velocity = new Vector3(
            (float)Math.Clamp(velocity.X, -maxValue, maxValue),
            (float)Math.Clamp(velocity.Y, -maxValue, maxValue),
            0
        );
        Transform.Position += velocity;

        float bounceVel = 0.05f;

        float distanceToLeft = Math.Abs(Transform.Position.X - leftEdge);
        float distanceToRight = Math.Abs(Transform.Position.X - rightEdge);
        float distanceToTop = Math.Abs(Transform.Position.Y - topEdge);
        float distanceToBottom = Math.Abs(Transform.Position.Y - bottomEdge);

        if (distanceToLeft < 10)
        {
            velocity += new Vector3(bounceVel / Math.Max(distanceToLeft, 0.01f), 0);
        }
        else if (distanceToRight < 10)
        {
            velocity += new Vector3(-bounceVel / Math.Max(distanceToRight, 0.01f), 0);
        }

        if (distanceToBottom < 10)
        {
            velocity += new Vector3(0, bounceVel / Math.Max(distanceToBottom, 0.01f));
        }
        else if (distanceToTop < 10)
        {
            velocity += new Vector3(0, -bounceVel / Math.Max(distanceToTop, 0.01f));
        }

        float angle = (float)
            Vector3.AngleBetween(Transform.Position, Transform.Position + velocity * 10);
        float deg = angle * (float)(180 / Math.PI);

        Transform.Rotation = new Vector3(0, 0, deg + 90);
    }

    private void SetScreenEdges()
    {
        Vector3 windowSize = new(
            MilliaGame.Instance.Window.Width,
            MilliaGame.Instance.Window.Height
        );

        Vector3 topLeft = Input.GetWorldCoordinatesFromScreen(new Vector3(0, 0));
        Vector3 bottomRight = Input.GetWorldCoordinatesFromScreen(
            new Vector3(windowSize.X, windowSize.Y)
        );

        leftEdge = topLeft.X;
        topEdge = topLeft.Y;
        bottomEdge = bottomRight.Y;
        rightEdge = bottomRight.X;
    }

    private HashSet<Transform> GetNearbyBoids(float range)
    {
        return allBoids
            .Where(boid => Vector3.Distance(boid.Position, Transform.Position) < range)
            .Where(x => x != Transform)
            .ToHashSet();
    }

    private void Separation()
    {
        // Negative sum of all vectors between bi and bj in the visible radius
        Vector3 sum = new();

        foreach (Transform visibleBoid in GetNearbyBoids(separationRange))
        {
            sum +=
                (visibleBoid.Position - Transform.Position)
                / (float)Vector3.Distance(visibleBoid.Position, Transform.Position);
        }

        separation = -sum;
    }

    private void Cohesion()
    {
        var nearbyBoids = GetNearbyBoids(cohesionRange);
        Vector3 sum = new();
        // Vector between center of flock and self
        foreach (Vector3 pos in nearbyBoids.Select(x => x.Position))
        {
            sum += pos;
        }

        if (nearbyBoids.Count > 1)
            sum /= nearbyBoids.Count;

        cohesion = sum - Transform.Position;
    }

    private void Alignment()
    {
        var nearbyBoids = GetNearbyBoids(alignmentRange);
        Vector3 sum = new();

        foreach (Vector3 pos in nearbyBoids.Select(x => x.Actor.GetComponent<Boid>().velocity))
        {
            sum += pos;
        }

        if (nearbyBoids.Count > 1)
            sum /= nearbyBoids.Count;

        alignment = sum - velocity;
    }
}
