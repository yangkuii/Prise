var target = Argument("target", "default");
var configuration = Argument("configuration", "Debug");
var plugins = new[] { "PluginA", "PluginB", "PluginC" };

private void CleanProject(string projectDirectory){
    var projectFile = $"IntegrationTestsPlugins/{projectDirectory}/{projectDirectory}.csproj";
    var bin = $"IntegrationTestsPlugins/{projectDirectory}/bin";
    var obj = $"IntegrationTestsPlugins/{projectDirectory}/obj";

    var deleteSettings = new DeleteDirectorySettings{
        Force= true,
        Recursive = true
    };

    var cleanSettings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };
    if (DirectoryExists(bin))
    {
      DeleteDirectory(bin, deleteSettings);
    }
    if (DirectoryExists(obj))
    {
      DeleteDirectory(obj, deleteSettings);
    }
    DotNetCoreClean(projectFile, cleanSettings);
}

Task("clean").Does( () =>
{ 
  foreach (var plugin in plugins)
  {
    CleanProject(plugin);
  }
});

Task("build")
  .IsDependentOn("clean")
  .Does( () =>
{ 
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    foreach (var plugin in plugins)
    {
      DotNetCoreBuild($"IntegrationTestsPlugins/{plugin}/{plugin}.csproj", settings);
    }
});

Task("publish")
  .IsDependentOn("build")
  .Does(() =>
  { 
    foreach (var plugin in plugins)
    {
      DotNetCorePublish($"IntegrationTestsPlugins/{plugin}/{plugin}.csproj", new DotNetCorePublishSettings
      {
          NoBuild = true,
          Configuration = configuration,
          OutputDirectory = $"publish/{plugin}"
      });
    }
  });

Task("copy-to-testhost")
  .IsDependentOn("publish")
  .Does(() =>
  {
    foreach (var plugin in plugins)
    {
      CopyDirectory($"publish/{plugin}", $"Prise.IntegrationTests/bin/debug/netcoreapp2.1/Plugins/{plugin}");
      CopyDirectory($"publish/{plugin}", $"Prise.IntegrationTestsHost/bin/debug/netcoreapp2.1/Plugins/{plugin}");
      CopyDirectory($"publish/{plugin}", $"Prise.IntegrationTests/bin/debug/netcoreapp3.0/Plugins/{plugin}");
      CopyDirectory($"publish/{plugin}", $"Prise.IntegrationTestsHost/bin/debug/netcoreapp3.0/Plugins/{plugin}");
    }
  });

Task("default")
  .IsDependentOn("copy-to-testhost");

RunTarget(target);