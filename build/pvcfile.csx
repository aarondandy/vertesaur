using System.Reflection;

var buildConfigurationName = "Debug";

public static IEnumerable<PvcStream> ExtractStreams(PvcPipe pipe){
    IEnumerable<PvcStream> capture = null;
    pipe.Pipe(streams => {
        capture = streams;
        return new PvcStream[0];
    });
    return capture;
}

pvc.Task("build", () => {
    pvc
        .Source("../vertesaur.sln")
        .Pipe(new PvcMSBuild(
            buildTarget: "Clean;Build",
            enableParallelism: true,
            configurationName : buildConfigurationName
        ));
});

pvc.Task("nuget-pack", () => {
    var pushDir = Directory.GetCurrentDirectory();
    Directory.SetCurrentDirectory(String.Format("../artifacts/bin/{0}/net40-client/", buildConfigurationName));

    var threePartVersionString = ExtractStreams(pvc.Source("*.dll"))
        .Select(stream => {
            Console.WriteLine(stream.OriginalSourcePath);
            var bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, bytes.Length);
            var asm = Assembly.ReflectionOnlyLoad(bytes);
            return asm.GetName().Version;
        })
        .Select(v => String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build))
        .Distinct()
        .Single();

    Directory.SetCurrentDirectory(pushDir);

    if(!Directory.Exists("../artifacts/nuget"))
        Directory.CreateDirectory("../artifacts/nuget");
    
    var nuspecSources = pvc.Source("*.nuspec");

    Directory.SetCurrentDirectory("../artifacts/nuget");
    nuspecSources
        .Pipe(new PvcNuGetPack(
            createSymbolsPackage: true,
            version: threePartVersionString,
            properties: String.Format("Configuration={0}", buildConfigurationName)
        ));

    Directory.SetCurrentDirectory(pushDir);
});

pvc.Task("release", () => {
    buildConfigurationName = "Release";
    pvc.Start("build");
    pvc.Start("nuget-pack");
});
pvc.Task("default").Requires("build");
