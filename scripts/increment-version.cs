using System.Xml.Linq;

string prName = Environment.GetEnvironmentVariable("PR_NAME");
int major,
    minor,
    patch;

XDocument doc = XDocument.Load("./Millia2D/Millia2d.csproj");
XElement versionElement = doc.Root.Element("Version");
string version = versionElement.Value;

// version can be something like 0.1.0
major = version.Split(".")[0];
minor = version.Split(".")[1];
patch = version.Split(".")[2];

List<string> prNameParts = prName.Split(":").ToList();
string prefix = prNameParts[0];

if (prName.Contains("chore:"))
{
    return;
}
else if (prName.Contains("!"))
{
    major++;
}
else
{
    switch (prefix.ToLower())
    {
        case "feat":
            minor++;
            break;
        case "fix":
            patch++;
            break;
    }
}

versionElement.Value = $"{major}.{minor}.{patch}";
doc.Save();
