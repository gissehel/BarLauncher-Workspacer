rm -rf */bin */obj

dotnet.exe build BarLauncher-Workspacer.sln
dotnet.exe publish BarLauncher.Workspacer.Wox/BarLauncher.Workspacer.Wox.csproj -c Release
dotnet.exe publish BarLauncher.Workspacer.Flow.Launcher/BarLauncher.Workspacer.Flow.Launcher.csproj -c Release -r win-x64

