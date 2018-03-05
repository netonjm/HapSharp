all: 
	#updating the submodules
	git submodule update --init --recursive

		# nuget restoring
	if [ ! -f src/.nuget/nuget.exe ]; then \
		mkdir -p src/.nuget ; \
	    echo "nuget.exe not found! downloading latest version" ; \
	    curl -O https://dist.nuget.org/win-x86-commandline/latest/nuget.exe ; \
	    mv nuget.exe src/.nuget/ ; \
	fi
	mono src/.nuget/nuget.exe restore HapSharp.sln
	msbuild HapSharp.sln /p:Configuration=Debug /p:Platform="x86"

install-mac:
	#installation of brew
	/usr/bin/ruby -e "$$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
	#installation of node
	brew install node
	
configure:
	#updating the submodules
	git submodule update --init --recursive
	#updates all the submodules
	cd HAP-NodeJS && npm install && npm update && npm install mqtt && cd ..

clean:
	echo "removing bin, obj and packages directories ..."
	find . -type d -name bin -exec rm -rf {} \;
	find . -type d -name obj -exec rm -rf {} \;
	find . -type d -name packages -exec rm -rf {} \;
	echo "killing possible broker local instancess opened..."
	killall nohup mono HapSharp.Host.Broker.exe
	killall mono HapSharp.Host.Console/bin/Debug/HapSharp.Host.Console.exe $(PWD)/HAP-NodeJS

broker:
	echo "Executing a new broker instance..."
	nohup mono HapSharp.Host.Console/bin/Debug/HapSharp.Host.Broker.exe &

run: all broker
	echo "Executing HAP-Sharp..."
	mono HapSharp.Host.Console/bin/Debug/HapSharp.Host.Console.exe $(PWD)/HAP-NodeJS
	
processes:
	sudo lsof -iTCP:51826 -sTCP:LISTEN

package:
	mono src/.nuget/nuget.exe restore HapSharp.sln
	msbuild HapSharp.sln /p:Configuration=Package
	curl -O https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
	mono nuget.exe pack NuGeT/HapSharp.nuspec
	mono nuget.exe pack NuGeT/HapSharp.Core.nuspec
	mono nuget.exe pack NuGeT/HapSharp.Accessories.HumidityExample.nuspec

.PHONY: all configure
