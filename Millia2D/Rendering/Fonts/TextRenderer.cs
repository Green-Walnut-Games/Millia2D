using BlurgText;
using Millia2D;
using Millia2D.Rendering.Shaders;
using Millia2D.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Vector3 = Millia2D.Mathematics.Vector3;

internal class UIRenderer : IDisposable
{
    public HashSet<Label> Labels { get; private set; } = new();

    private Vertex[] vertices = new Vertex[16384];
    private int vCount = 0;
    private IntPtr currentTexture = IntPtr.Zero;

    private Shader shader;
    private int vao;
    private int vbo;

    internal Blurg blurg;

    private List<int> textures = new();

    internal void Init()
    {
        blurg = new Blurg(CreateTexture, UpdateTexture);
        blurg.EnableSystemFonts();
    }

    internal void OnLoad()
    {
        foreach (var pair in LoadedFonts)
        {
            Log.Debug($"Loaded font: {pair.Key}");
        }

        shader = new("assets/shaders/font-shader.vert", "assets/shaders/font-shader.frag");
        shader.Set("texture", 0);

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            (IntPtr)(5 * sizeof(float) * 16384),
            IntPtr.Zero,
            BufferUsageHint.StreamDraw
        );
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(
            1,
            2,
            VertexAttribPointerType.Float,
            false,
            5 * sizeof(float),
            2 * sizeof(float)
        );
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(
            2,
            4,
            VertexAttribPointerType.UnsignedByte,
            true,
            5 * sizeof(float),
            4 * sizeof(float)
        );
    }

    internal void OnRenderFrame()
    {
        Vector3 ClientSize = new(
            MilliaGame.Instance.Window.Width,
            MilliaGame.Instance.Window.Height
        );

        var sz = ClientSize;
        GL.Viewport(0, 0, (int)sz.X, (int)sz.Y);

        // Setup UI viewport
        Start((int)sz.X, (int)sz.Y);

        // 0,0 is top left of the screen.

        foreach (Label label in Labels)
        {
            RenderLabel(label);
        }

        Finish();
    }

    internal Dictionary<string, BlurgFont> LoadedFonts = new();

    internal void RenderLabel(Label label)
    {
        BlurgColor color = new(label.Color.R, label.Color.G, label.Color.B, label.Color.A);

        BlurgColor backgroundColor = new(
            label.BackgroundColor.R,
            label.BackgroundColor.G,
            label.BackgroundColor.B,
            label.BackgroundColor.A
        );

        BlurgFont font = blurg.QueryFont(label.FontFace, FontWeight.Bold, false)!;

        if (font == null)
        {
            throw new Exception("Font object could not be created for massiv");
        }

        BlurgResult result = blurg.BuildString(font, (float)label.Size, color, label.Text);
        System.Numerics.Vector2 textSize = blurg.MeasureString(font, (float)label.Size, label.Text);
        label.Width = (int)textSize.X;
        label.Height = (int)textSize.Y;

        if (label.Background)
        {
            label.BackgroundWidth = label.Width + label.BackgroundPadding;
            label.BackgroundHeight = label.Height + label.BackgroundPadding;

            if (label is Button)
            {
                Button button = label as Button;

                BlurgColor hoverColor = new(
                    button.HoverColor.R,
                    button.HoverColor.G,
                    button.HoverColor.B,
                    button.HoverColor.A
                );

                FillBackground(
                    (int)label.Transform.Position.X - label.BackgroundPadding / 2,
                    (int)label.Transform.Position.Y - label.BackgroundPadding / 2,
                    (int)result.Width + label.BackgroundPadding,
                    (int)result.Height + label.BackgroundPadding,
                    button.isHovered ? hoverColor : backgroundColor
                );
            }
            else
            {
                FillBackground(
                    (int)label.Transform.Position.X - label.BackgroundPadding / 2,
                    (int)label.Transform.Position.Y - label.BackgroundPadding / 2,
                    (int)result.Width + label.BackgroundPadding,
                    (int)result.Height + label.BackgroundPadding,
                    backgroundColor
                );
            }
        }

        DrawRects(result, (int)label.Transform.Position.X, (int)label.Transform.Position.Y);
    }

    internal BlurgFont LoadFontFromFile(string path)
    {
        BlurgFont font = blurg.AddFontFile(path);
        if (font == null)
        {
            throw new Exception($"Font with path {path} could not be found.");
        }

        LoadedFonts.Add(font.FamilyName, font);

        return font;
    }

    internal BlurgFont LoadFont(string familyName)
    {
        return LoadedFonts[familyName];
    }

    internal void Start(int width, int height)
    {
        GL.Disable(EnableCap.CullFace);
        GL.Disable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        shader.Use();
        var m = Matrix4.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
        shader.Set("viewprojection", m);
    }

    public void DrawRects(BlurgResult? rects, int x, int y)
    {
        if (rects == null)
            return;
        for (int i = 0; i < rects.Count; i++)
        {
            if (
                vCount + 6 > vertices.Length
                || (currentTexture != rects[i].UserData && currentTexture != IntPtr.Zero)
            )
                Flush();
            currentTexture = rects[i].UserData;
            var o = new Vector2(x, y);
            var pos = new Vector2(rects[i].X, rects[i].Y);
            var tl = new Vertex(o + pos, new Vector2(rects[i].U0, rects[i].V0), rects[i].Color);
            var tr = new Vertex(
                o + pos + new Vector2(rects[i].Width, 0),
                new Vector2(rects[i].U1, rects[i].V0),
                rects[i].Color
            );
            var bl = new Vertex(
                o + pos + new Vector2(0, rects[i].Height),
                new Vector2(rects[i].U0, rects[i].V1),
                rects[i].Color
            );
            var br = new Vertex(
                o + pos + new Vector2(rects[i].Width, rects[i].Height),
                new Vector2(rects[i].U1, rects[i].V1),
                rects[i].Color
            );
            vertices[vCount++] = tl;
            vertices[vCount++] = tr;
            vertices[vCount++] = bl;
            vertices[vCount++] = bl;
            vertices[vCount++] = tr;
            vertices[vCount++] = br;
        }
    }

    public void FillBackground(int x, int y, int width, int height, BlurgColor color)
    {
        if (vCount + 6 > vertices.Length)
            Flush();
        var tcoord = new Vector2(0.5f / 1024.0f, 0.5f / 1024.0f);
        var tl = new Vertex(new Vector2(x, y), tcoord, color);
        var tr = new Vertex(new Vector2(x + width, y), tcoord, color);
        var bl = new Vertex(new Vector2(x, y + height), tcoord, color);
        var br = new Vertex(new Vector2(x + width, y + height), tcoord, color);
        vertices[vCount++] = tl;
        vertices[vCount++] = tr;
        vertices[vCount++] = bl;
        vertices[vCount++] = bl;
        vertices[vCount++] = tr;
        vertices[vCount++] = br;
    }

    void Flush()
    {
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferSubData(
            BufferTarget.ArrayBuffer,
            IntPtr.Zero,
            vCount * sizeof(float) * 5,
            vertices
        );
        GL.BindTexture(TextureTarget.Texture2D, (uint)currentTexture);
        GL.DrawArrays(PrimitiveType.Triangles, 0, vCount);
        vCount = 0;
    }

    public void Finish()
    {
        if (vCount > 0)
            Flush();
    }

    internal void AddText(Label label)
    {
        Labels.Add(label);
    }

    internal void RemoveText(Label label)
    {
        Labels.Remove(label);
    }

    private IntPtr CreateTexture(int width, int height)
    {
        var texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            width,
            height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            IntPtr.Zero
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)TextureWrapMode.ClampToEdge
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)TextureWrapMode.ClampToEdge
        );
        textures.Add(texture);
        return (IntPtr)texture;
    }

    private void UpdateTexture(IntPtr texture, IntPtr buffer, int x, int y, int width, int height)
    {
        GL.BindTexture(TextureTarget.Texture2D, (uint)texture);
        GL.TexSubImage2D(
            TextureTarget.Texture2D,
            0,
            x,
            y,
            width,
            height,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            buffer
        );
    }

    public void Dispose() { }
}
