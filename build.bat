@echo off 

dotnet publish .\G12App.API\ -c Release --runtime ubuntu.22.04-x64 --self-contained -o .\ubuntu-build\

pause