using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Rocket.Surgery.Nuke.DotNetCore;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class DuneBuild : NukeBuild,
                  ICanRestoreWithDotNetCore,
                  ICanTestWithDotNetCore,
                  ICanBuildWithDotNetCore,
                  IHaveConfiguration<Configuration>
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<DuneBuild>(build => build.Build);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public Configuration Configuration { get; } = IsLocalBuild
                                                      ? Configuration.Debug
                                                      : Configuration.Release;

    public Target Clean => _ => _.Before(Restore)
                                 .Inherit<ICanClean>(x => x.Clean);

    public Target Restore => _ => _.Inherit<ICanRestoreWithDotNetCore>(dotNetCore => dotNetCore.CoreRestore);

    public Target Build => _ =>  _.Inherit<ICanBuildWithDotNetCore>(dotNetCore => dotNetCore.CoreBuild)
                                  .DependsOn(Restore);

    public Target Test => _ => _.Executes();

    Target Benchmarks => _ => _
                              .DependsOn(Build)
                              .Executes(() =>
                              {
                                  var project =
                                      ((IHaveSolution) this).Solution.AllProjects.First(x => x.Name == "Performance");

                                  DotNetRun(configurator => configurator
                                                            .SetConfiguration(Configuration.Release)
                                                            .SetProjectFile(project.Path)
                                                            .SetProcessArgumentConfigurator(
                                                                argumentConfigurator =>
                                                                    argumentConfigurator.Add("--filter *")));
                              });

    public GitVersion GitVersion { get; }

    public bool CollectCoverage { get; }
}