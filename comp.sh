rm -rf */bin */obj */build build

dotnet.exe publish BarLauncher.Workspacer.Wox/BarLauncher.Workspacer.Wox.csproj -c Debug
dotnet.exe publish BarLauncher.Workspacer.Flow.Launcher/BarLauncher.Workspacer.Flow.Launcher.csproj -c Debug -r win-x64

