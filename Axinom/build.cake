var target = Argument("target", "Default");
var configuration = "Debug";

Task("Restore")
    .Does(() => {
        DotNetCoreRestore();
    });
Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
      // Use MSBuild
      MSBuild("./Axinom.sln", settings =>
        settings.SetConfiguration(configuration));

});
Task("UnitTest")
    .Does(() => {
        foreach(var proj in GetFiles("./**/*.Tests.csproj")) {
            DotNetCoreTest(proj.ToString(), new DotNetCoreTestSettings {
                NoBuild = true,
                Configuration = configuration
            });
        }
    });

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest");

RunTarget(target);