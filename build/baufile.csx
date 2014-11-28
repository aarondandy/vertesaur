// parameters
var msBuildFileVerbosity = (BauMSBuild.Verbosity)Enum.Parse(typeof(BauMSBuild.Verbosity), Environment.GetEnvironmentVariable("MSBUILD_FILE_VERBOSITY") ?? "minimal", true);
var nugetVerbosity = (BauNuGet.Verbosity)Enum.Parse(typeof(BauNuGet.Verbosity), Environment.GetEnvironmentVariable("NUGET_VERBOSITY") ?? "quiet", true);

// solution specific variables
var versionParts = File.ReadAllText("../src/VertesaurAssemblyInfo.cs")
    .Split(new[] { "AssemblyVersion(\"" }, 2, StringSplitOptions.None)
    .ElementAt(1)
    .Split(new[] { '"' })
    .First()
    .Split(new[] { '.' })
    .Take(3);
var version = String.Join(".", versionParts);
var solution = "../vertesaur.sln";
var buildConfigurationName = "Debug";
var artifactFolder = "../artifacts";
var nugetOutputFolder = Path.Combine(artifactFolder, "nuget");
var logsFolder = Path.Combine(artifactFolder, "logs");

private string GetVersionSuffix(){
    var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX");
    if(!String.IsNullOrEmpty(versionSuffix))
        return versionSuffix;
    if("Release".Equals(buildConfigurationName, StringComparison.OrdinalIgnoreCase))
        return String.Empty;
    return "-adhoc";
}

var bau = Require<Bau>();
bau

.Task("default").DependsOn("build", "pack")

.Task("release").DependsOn("set-release", "default")

.Task("set-release").Do(() => {buildConfigurationName = "Release";})

.NuGet("restore").Do(nuget => nuget.Restore(solution))

.MSBuild("build")
.DependsOn("restore", "create-artifact-folders")
.Do(msb => {
    msb.MSBuildArchitecture = System.Reflection.ProcessorArchitecture.X86; // required for building ports
    msb.Solution = solution;
    msb.Targets = new[] { "Clean", "Build" };
    msb.Properties = new { Configuration = buildConfigurationName };
    msb.Verbosity = BauMSBuild.Verbosity.Minimal;
    msb.NoLogo = true;
    msb.FileLoggers.Add(new FileLogger {
        FileLoggerParameters = new FileLoggerParameters {
            PerformanceSummary = true,
            Summary = true,
            Verbosity = msBuildFileVerbosity,
            LogFile = Path.Combine(logsFolder,"build.log")
        }
    });
})

.NuGet("pack")
.DependsOn("create-artifact-folders")
.Do(nuget => nuget.Pack(
    Directory.EnumerateFiles("./", "*.nuspec"),
    r => r
        .WithOutputDirectory("../artifacts/nuget")
        .WithProperty("Configuration", buildConfigurationName)
        .WithIncludeReferencedProjects()
        .WithVerbosity(nugetVerbosity)
        .WithVersion(version + GetVersionSuffix())
))

.Task("create-artifact-folders").Do(() => {
    var directories = new[]{
        artifactFolder,
        nugetOutputFolder,
        logsFolder
    }.Select(d => new DirectoryInfo(d));
    foreach(var directory in directories.Where(di => !di.Exists)){
        directory.Create();
    }
    System.Threading.Thread.Sleep(100);
})

.Run();