using Millia2d.Resources;
using Millia2d.Resources.Loaders;

namespace Millia2D.Resources;

public class ResourceStore
{
    /// <summary>
    /// Cache for resources. Keyed by file path.
    /// </summary>
    public static Dictionary<string, object> ResourceCache { get; private set; } = new();

    public static Dictionary<string, IResourceLoader> ResourceLoaders { get; private set; } = new();

    public static string AssetsPath { get; set; } = "assets";

    static ResourceStore()
    {
        Console.WriteLine("Adding new loader for text");
        ResourceLoaders.Add(".txt", new TextLoader());
    }

    public static void AddResourceLoader<T>(string fileExtension, IResourceLoader loader)
    {
        ResourceLoaders.Add(fileExtension, loader);
    }

    public static T Load<T>(string filePath)
        where T : class
    {
        if (ResourceCache.ContainsKey(filePath))
        {
            return (T)ResourceCache[filePath];
        }

        string extension = Path.GetExtension(filePath);
        Console.WriteLine($"Loaders length is {ResourceLoaders.Count}");
        IResourceLoader loader = ResourceLoaders[extension];

        string correctedPath = Directory.GetCurrentDirectory() + "/" + filePath;

        object returnObject = loader.Load(correctedPath);

        if (returnObject == null)
        {
            throw new Exception($"Could not load {correctedPath}");
        }

        ResourceCache[filePath] = returnObject;

        return (T)returnObject;
    }
}
