using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Millia2d.Rendering;

public class Texture
{
    public int Handle { get; private set; }

    public Texture(string path)
    {
        Handle = GL.GenTexture();
        Use();

        StbImage.stbi_set_flip_vertically_on_load(1);

        ImageResult image = ImageResult.FromStream(
            File.OpenRead(path),
            ColorComponents.RedGreenBlueAlpha
        );

        // Have to set the minification filter to something. This is because
        // openTk assumes we are using mipmaps, and if we don't set anything, it errors
        // and it's all black.
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMagFilter.Nearest
        );
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            image.Width,
            image.Height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            image.Data
        );
    }

    internal void Use()
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}
