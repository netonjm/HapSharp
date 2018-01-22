#General vars

all: 
	msbuild HapSharp.sln /p:Configuration=Debug

configure: 
	git submodule sync && git submodule update --init --recursive --force
	cd HAP-NodeJS && npm install && npm update && npm install mqtt && cd ..

run: all
	mono HapSharp.Host.Console/bin/Debug/HapSharp.Host.Console.exe 
	
processes:
	sudo lsof -iTCP:51826 -sTCP:LISTEN

package: all
	curl -O https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
	mono nuget.exe pack NuGeT/HapSharp.nuspec

.PHONY: all configure
