using System.Drawing;
using Millia2D.Mathematics;

namespace Millia2D.UI;

public class Label : Component
{
    public string Text { get; set; } = "New text";
    public double Size { get; set; } = 20;
    public Color Color { get; set; } = Color.White;
    public bool Underline { get; set; } = false;
    public string FontFace { get; set; } = "Arial";
    public int Width { get; internal set; }
    public int Height { get; internal set; }

    // Background
    public bool Background { get; set; } = false;
    public Color BackgroundColor { get; set; } = Color.White;
    public int BackgroundPadding { get; set; } = 0;

    public int BackgroundWidth { get; internal set; }
    public int BackgroundHeight { get; internal set; }

    private void GetWidth() { }

    public override void Preload()
    {
        base.Preload();

        MilliaGame.Instance.UIRenderer.AddText(this);
    }

    public bool IsPositionWithinRect(Vector3 position)
    {
        // TODO: Change this to use full rect sizings, for now use just background rect
        // so buttons have to have a background to be clickable

        Log.Info($"Click position: {position}");

        int topLeftX = (int)Transform.Position.X - BackgroundPadding / 2;
        int topLeftY = (int)Transform.Position.Y - BackgroundPadding / 2;

        int botRightX = (int)Transform.Position.X + BackgroundWidth + BackgroundPadding / 2;
        int botRightY = (int)Transform.Position.Y + BackgroundHeight + BackgroundPadding / 2;

        Log.Info($"Bounds: {topLeftX}, {topLeftY} and {botRightX}, {botRightY}");

        if (position.X < topLeftX || position.X > botRightX)
        {
            return false;
        }

        if (position.Y < topLeftY || position.Y > botRightY)
        {
            return false;
        }

        return true;
    }

    // TODO: Add destroy for components to dispose of resources
}
