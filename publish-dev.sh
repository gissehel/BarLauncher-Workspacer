#!/usr/bin/env bash

rm -rf ./*/bin ./*/obj ./build

VERSION=$(cat VERSION)-$(date +%s)

dotnet.exe publish BarLauncher.Workspacer.Wox/BarLauncher.Workspacer.Wox.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.Workspacer.Wox/bin/Debug/net48/publish; zip -r ../../../../../../../BarLauncher-Workspacer-${VERSION}.wox .)

dotnet.exe publish BarLauncher.Workspacer.Flow.Launcher/BarLauncher.Workspacer.Flow.Launcher.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.Workspacer.Flow.Launcher/bin/Debug/net5.0-windows/publish; zip -r ../../../../../../../BarLauncher-Workspacer-${VERSION}.zip .)
