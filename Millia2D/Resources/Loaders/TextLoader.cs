namespace Millia2d.Resources.Loaders;

public class TextLoader : IResourceLoader
{
    public object Load(string filePath)
    {
        return new StreamReader(filePath).ReadToEnd();
    }
}
