#!/bin/bash

# build the solution
cd ../
dotnet build RightMove.sln --configuration Release
tar -czvf rightmove.tar.gz RightMoveConsole/bin/Release/net8.0/.