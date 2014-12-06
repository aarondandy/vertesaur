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
var buildConfigurationName = "Debug";
var buildDir = new DirectoryInfo("./");
var buildPackagesDir = new DirectoryInfo("packages");
var artifactDir = new DirectoryInfo("../artifacts");
var nugetOutputDir = new DirectoryInfo(Path.Combine(artifactDir.FullName, "nuget"));
var logsDir = new DirectoryInfo(Path.Combine(artifactDir.FullName, "logs"));
var repositoryDir = new DirectoryInfo("../");
var solutionFile = new FileInfo(Path.Combine(repositoryDir.FullName,"vertesaur.sln"));

// helpers
private string GetVersionSuffix(){
    var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX");
    if(!String.IsNullOrEmpty(versionSuffix))
        return versionSuffix;
    if("Release".Equals(buildConfigurationName, StringComparison.OrdinalIgnoreCase))
        return String.Empty;
    return "-adhoc";
}
private DirectoryInfo BinaryOutputFolder { get {
    return new DirectoryInfo(Path.Combine(artifactDir.FullName, "bin", buildConfigurationName));
} }

// tasks
var bau = Require<Bau>();
bau

.Task("default").DependsOn("build", "test")

.Task("release").DependsOn("set-release", "default", "pack")

.Task("set-release").Do(() => {buildConfigurationName = "Release";})

.NuGet("restore").Do(nuget => nuget.Restore(solutionFile.FullName))

.MSBuild("build")
.DependsOn("restore", "create-artifact-folders")
.Do(msb => {
    msb.MSBuildArchitecture = System.Reflection.ProcessorArchitecture.X86; // required for building ports
    msb.Solution = solutionFile.FullName;
    msb.Targets = new[] { "Clean", "Build" };
    msb.Properties = new { Configuration = buildConfigurationName };
    msb.Verbosity = BauMSBuild.Verbosity.Minimal;
    msb.NoLogo = true;
    msb.FileLoggers.Add(new FileLogger {
        FileLoggerParameters = new FileLoggerParameters {
            PerformanceSummary = true,
            Summary = true,
            Verbosity = msBuildFileVerbosity,
            LogFile = Path.Combine(logsDir.FullName,"build.log")
        }
    });
})

.NuGet("pack")
.DependsOn("create-artifact-folders")
.Do(nuget => nuget.Pack(
    Directory.EnumerateFiles(buildDir.FullName, "*.nuspec"),
    r => r
        .WithOutputDirectory(nugetOutputDir.FullName)
        .WithProperty("Configuration", buildConfigurationName)
        .WithIncludeReferencedProjects()
        .WithVerbosity(nugetVerbosity)
        .WithVersion(version + GetVersionSuffix())
		.WithSymbols()
))

.Task("test").DependsOn("xunit")

.Xunit("xunit")
.DependsOn("create-artifact-folders")
.Do(xunit => {
    xunit.Exe = buildPackagesDir
		.EnumerateDirectories("xunit.runners.*").Single()
		.EnumerateDirectories("tools").Single()
        .EnumerateFiles("xunit.console.exe").Single()
        .FullName;
    xunit.Assemblies = BinaryOutputFolder.EnumerateFiles("*.Test.dll").Select(f => f.FullName);
})

.Task("create-artifact-folders").Do(() => {
    var dirsToCreate = new[] {
        artifactDir,
        nugetOutputDir,
        logsDir
    }.Where(di => !di.Exists);
    foreach(var di in dirsToCreate) {
        di.Create();
    }
    System.Threading.Thread.Sleep(100);
})

.Run();