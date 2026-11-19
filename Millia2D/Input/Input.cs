namespace Millia2D.Inputs;

using Millia2D.Mathematics;
using Millia2D.Rendering;
using OTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;
using OTKMouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using Vector2 = OpenTK.Mathematics.Vector2;

/// <summary>
/// Class used to detect input inside the engine.
/// </summary>
public static class Input
{
    /// <summary>
    /// Returns true when the specified key is being held down in the current frame.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyDown(Keys key)
    {
        return MilliaGame.Instance.Window.otk_Window.IsKeyDown((OTKKeys)(int)key);
    }

    /// <summary>
    /// Returns true when the specified key has just been pressed inside this frame.
    /// Will not return true if the key was pressed in a previous frame.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyPressed(Keys key)
    {
        return MilliaGame.Instance.Window.otk_Window.IsKeyPressed((OTKKeys)(int)key);
    }

    /// <summary>
    /// Returns true when the specified key has just been released this frame.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsKeyReleased(Keys key)
    {
        return MilliaGame.Instance.Window.otk_Window.IsKeyReleased((OTKKeys)(int)key);
    }

    /// <summary>
    /// Checks whether there are any inputs active this frame.
    /// </summary>
    /// <returns></returns>
    public static bool IsAnyKeyDown()
    {
        return MilliaGame.Instance.Window.otk_Window.IsAnyKeyDown;
    }

    /// <summary>
    /// Checks whether the specified mouse button is being held down in the current frame.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonDown(MouseButton button)
    {
        return MilliaGame.Instance.Window.otk_Window.IsMouseButtonDown((OTKMouseButton)(int)button);
    }

    /// <summary>
    /// Returns true when the specified mouse button has just been pressed inside this frame.
    /// Will not return true if the mouse button was pressed in a previous frame.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonPressed(MouseButton button)
    {
        return MilliaGame.Instance.Window.otk_Window.IsMouseButtonPressed(
            (OTKMouseButton)(int)button
        );
    }

    /// <summary>
    /// Returns true when the specified mouse button has just been released this frame.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonReleased(MouseButton button)
    {
        return MilliaGame.Instance.Window.otk_Window.IsMouseButtonReleased(
            (OTKMouseButton)(int)button
        );
    }

    public static Vector3 GetMousePosition()
    {
        Vector2 screenPosition = MilliaGame.Instance.Window.otk_Window.MousePosition;
        return new Vector3(screenPosition.X, screenPosition.Y);
    }

    internal static Dictionary<string, (Keys positive, Keys negative)> InputAxis
    {
        get;
        private set;
    } = new();

    /// <summary>
    /// Allows you to create an axis of movement. For example, you can create
    /// a "Horizontal" axis with the positive value of D and negative of A.
    /// This will allow you to use GetAxis("Horizontal") and receive
    /// either -1, 0, or 1 depending on the input.
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="positive"></param>
    /// <param name="negative"></param>
    public static void DefineAxis(string axisName, Keys positive, Keys negative)
    {
        if (InputAxis.ContainsKey(axisName))
        {
            Log.Warning($"Axis already defined: {axisName}");
            return;
        }

        InputAxis.Add(axisName, (positive, negative));
    }

    /// <summary>
    /// Allows you to query a defined axis of movement.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns>-1, 0, or 1 depending on the input.</returns>
    public static double GetAxis(string axisName)
    {
        if (!InputAxis.ContainsKey(axisName))
        {
            Log.Warning($"Axis {axisName} is not defined.");
        }

        (Keys positive, Keys negative) axis = InputAxis[axisName];

        if (IsKeyDown(axis.positive) && IsKeyDown(axis.negative))
        {
            return 0;
        }
        else if (IsKeyDown(axis.positive))
        {
            return 1;
        }
        else if (IsKeyDown(axis.negative))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static Vector3 GetScreenCoordinatesFromWorld(Vector3 coords)
    {
        return Renderer.GetScreenCoordinatesFromWorld(coords);
    }

    public static Vector3 GetWorldCoordinatesFromScreen(Vector3 coords)
    {
        return Renderer.GetWorldCoordinatesFromScreen(coords);
    }
}
