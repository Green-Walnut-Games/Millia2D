using System.Xml.Linq;

string prName = Environment.GetEnvironmentVariable("PR_NAME").ToLower();
int major,
    minor,
    patch;

string githubEnvPath = Environment.GetEnvironmentVariable("GITHUB_ENV");
string githubOutputPath = Environment.GetEnvironmentVariable("GITHUB_OUTPUT");

if (prName.Contains("chore: autopublish"))
{
    Skip();
}

string csProjPath = "Millia2D/Millia2D.csproj";

XDocument doc = XDocument.Load(csProjPath);
XElement versionElement = doc.Root.Element("PropertyGroup").Element("Version");
string version = versionElement.Value;

// version can be something like 0.1.0
// if extra text is needed for version numbers
// it can easily be achieved using 1.0.2.3-dev5 - the fourth dot
major = Int32.Parse(version.Split(".")[0]);
minor = Int32.Parse(version.Split(".")[1]);
patch = Int32.Parse(version.Split(".")[2]);

List<string> prNameParts = prName.Split(":").ToList();
string prefix = prNameParts[0];

if (!prName.Contains(":"))
{
    Console.WriteLine("Unexpected PR title format.");
    Environment.Exit(1);
}

if (prName.Contains("chore:") || prName.Contains("docs:"))
{
    Skip();
}
else if (prName.Contains("!"))
{
    major++;
    minor = 0;
    patch = 0;
}
else
{
    if (prefix.Contains("feat"))
    {
        minor++;
        patch = 0;
    }
    else if (prefix.Contains("fix"))
    {
        patch++;
    }
}

string finalVersion = $"{major}.{minor}.{patch}";
versionElement.Value = finalVersion;
doc.Save(csProjPath);

File.AppendAllText(githubEnvPath, $"FINAL_VERSION={finalVersion}{Environment.NewLine}");
File.AppendAllText(githubOutputPath, $"skip=false{Environment.NewLine}");

void Skip()
{
    File.AppendAllText(githubOutputPath, $"skip=true{Environment.NewLine}");
    Environment.Exit(0);
}
