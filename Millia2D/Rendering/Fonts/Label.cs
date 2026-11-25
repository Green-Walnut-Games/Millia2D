using System.Drawing;
using Millia2D;

public class Label : Component
{
    public string Text { get; set; } = "New text";
    public double Size { get; set; } = 20;
    public Color Color { get; set; } = Color.White;
    public bool Underline { get; set; } = false;
    public string FontFace { get; set; } = "Arial";

    // Background
    public bool Background { get; set; } = false;
    public Color BackgroundColor { get; set; } = Color.White;
    public int BackgroundPadding { get; set; } = 0;

    public override void Preload()
    {
        base.Preload();

        MilliaGame.Instance.TextRenderer.AddText(this);
    }

    // TODO: Add destroy for components to dispose of resources
}
