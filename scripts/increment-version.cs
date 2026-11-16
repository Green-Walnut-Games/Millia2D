using System.Xml.Linq;

string prName = Environment.GetEnvironmentVariable("PR_NAME");
int major,
    minor,
    patch;

if (prName.Contains("chore: Autopublish"))
{
    return;
}

string csProjPath = "Millia2D/Millia2d.csproj";

XDocument doc = XDocument.Load(csProjPath);
XElement versionElement = doc.Root.Element("PropertyGroup").Element("Version");
string version = versionElement.Value;

// version can be something like 0.1.0
// if extra text is needed for version numbers,
// it can easily be achieved using 1.0.2.3-dev5 - the fourth dot
major = Int32.Parse(version.Split(".")[0]);
minor = Int32.Parse(version.Split(".")[1]);
patch = Int32.Parse(version.Split(".")[2]);

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
doc.Save(csProjPath);
