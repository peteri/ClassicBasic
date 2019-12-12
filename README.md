[![Build status](https://ci.appveyor.com/api/projects/status/a1jxfg0vfj5kuige?svg=true)](https://ci.appveyor.com/project/peteri/classicbasic)
[![codecov](https://codecov.io/gh/peteri/ClassicBasic/branch/master/graph/badge.svg)](https://codecov.io/gh/peteri/ClassicBasic)

# ClassicBasic - Clone of Microsoft 8K BASIC

This version of BASIC is designed to implement most of a Microsoft 8K basic interpreter in C#. 
The initial aim is to get the code to be able to run the contents of "BASIC computer games" by David Ahl. 
See the games directory for the files from the book.

## Installing and running
Currently not sorted out yet, build from source is the current advice.

## Build for linux
Install dotnet core 3.1 sdk (use these steps for ubuntu 19) or see https://docs.microsoft.com/en-gb/dotnet/core/install/linux-package-manager-ubuntu-1904 
```
wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-3.
```

Clone the git repo and build
```
git clone https://github.com/peteri/ClassicBasic.git
cd ClassicBasic/
git checkout UpgradeToCore3.1
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true ClassicBasic/ClassicBasic.csproj
```

Binary will be 
```
ClassicBasic/bin/Release/netcoreapp3.1/linux-x64/publish/ClassicBasic
```

"SYSTEM" will exit back to the shell.