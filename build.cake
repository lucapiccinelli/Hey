///////////////////////////////////////////////////////////////////////////////
// Addins
///////////////////////////////////////////////////////////////////////////////
#addin "Cake.Incubator"
#addin "Cake.Compression"
#tool "nuget:?package=NUnit.ConsoleRunner"
using SysPath = System.IO.Path;
using System.Diagnostics;

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var platform = Argument<PlatformTarget>("platform", PlatformTarget.x86);
var gitTagComment = Argument<string>("gitTagComment", string.Empty);

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutions = GetFiles("*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());
var serviceProject = solutions
    .Select(solution => ParseSolution(solution))
    .SelectMany(parsedSolution => parsedSolution.Projects)
    .Where(project => project.Name.Contains("Service") && !project.Name.Contains("Tests"))
    .FirstOrDefault();

string solutionName = solutions.Select(solution => SysPath.GetFileNameWithoutExtension(solution.ToString())).FirstOrDefault();

string packagePrefix = "";
string basePublishDir = @"\\192.168.0.9\Pubblicazioni";
string baseDeployDir = "_deploy";
string deployDir = baseDeployDir;

string packageNamePath = string.Empty;
string version = string.Empty;
string publishDir = string.Empty;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(cake =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
    Information($"Verra' compilato {serviceProject.Name} del progetto {solutionName}");
});

Teardown(cake =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        string binDir = path + "/**/bin/" + platform + "/" + configuration;
        string objDir = path + "/**/obj/" + platform + "/" + configuration;
        Information("Cleaning {0}", binDir);
        Information("Cleaning {0}", objDir);
        CleanDirectories(binDir);
        CleanDirectories(objDir);
    }
});

Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}...", solution);
        NuGetRestore(solution);
    }
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}. Configuration: {1}, platformTarget: {2}", solution, configuration, platform);
        MSBuild(solution, settings =>
            settings
                .SetPlatformTarget(platform)
                .WithProperty("TreatWarningsAsErrors","false")
                .WithTarget("Build")
                .SetConfiguration(configuration));
    }
});

Task("Deploy")
    .Description("Sposta tutto il necessario nella cartella di deploy")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(deployDir);
    CleanDirectory(deployDir);
    AssemblyInfoParseResult solutionInfo = ParseAssemblyInfo("SolutionInfo.cs");
    version = solutionInfo.AssemblyVersion;

    Information("Version {0}", version);
    Information("Parsing {0}", serviceProject.Path);
    CustomProjectParserResult parsedProject = ParseProject(serviceProject.Path, configuration: configuration, platform: platform.ToString());
    Information(
        @"Solution project file: Name: {0} Path: {1} output path: {2}",
        serviceProject.Name,
        serviceProject.Path,
        parsedProject.OutputPath
    );
    CopyDirectory(parsedProject.OutputPath, deployDir);
    // DeleteFiles(SysPath.Combine(deployDir, "*.xml"));
    // DeleteFiles(SysPath.Combine(deployDir, "*.pdb"));
    DeleteFiles(SysPath.Combine(deployDir, "*ConnectionStrings.config"));
    DeleteFiles(SysPath.Combine(deployDir, "*AppSettings.config"));
    System.IO.File.WriteAllText(SysPath.Combine(deployDir, $"{solutionName}.ver"), version);

    packageNamePath = SysPath.Combine(deployDir, $"{solutionName}{configuration}.zip");
    Zip(baseDeployDir, packageNamePath);

    publishDir = SysPath.Combine(basePublishDir, solutionName, version);
});

Task("Publish")
    .Description("Mette il pacchetto nella shared directory")
    .IsDependentOn("Deploy")
    .Does(() =>
{
    try
    {
        EnsureDirectoryExists(publishDir);
        CopyFileToDirectory(packageNamePath, publishDir);
        Information($"pubblicato nella directory {publishDir}");
    }
    catch (System.Exception ex)
    {
        Error($"Error while publishing in {publishDir}");
        Warning(ex.ToString());
    }

    if(gitTagComment != string.Empty)
    {
        string gitTag = $"v{version}";
        Information($"aggiunto tag git {gitTag} e commento \"{gitTagComment}\"");
        ProcessStartInfo gitTagInfo = new ProcessStartInfo()
        {
            Arguments = $"tag -a {gitTag} -m \"{gitTagComment}\"",
            FileName = "git",
            CreateNoWindow = true,
        };

        ProcessStartInfo gitPushInfo = new ProcessStartInfo()
        {
            Arguments = "push --tags",
            FileName = "git",
            CreateNoWindow = true,
        };

        using(Process tagProcess = Process.Start(gitTagInfo))
        {
            tagProcess.WaitForExit();
            if(tagProcess.ExitCode == 0)
            {
                using(Process pushProcess = Process.Start(gitPushInfo))
                {
                    pushProcess.WaitForExit();
                    if(pushProcess.ExitCode == 0)
                    {
                        Information($"Tag creato");
                    }
                    else
                    {
                        Error($"Errore nel push del tag");
                    }
                }
            }
            else
            {
                Error($"Errore nella creazione del tag");
            }
        }
    }
});

Task("Test")
    .Description("Esegue tutti i test NUnit")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var path in solutionPaths)
    {
        string testDir = path + "/**/bin/" + platform + "/" + configuration + "/*.Tests.dll";
        Information("Sto lanciando i test in {0}", testDir);
        NUnit3(testDir);
    }
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Publish");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);