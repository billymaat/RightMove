#!/bin/bash

# build the solution
echo $BUILD_NUMBER
dotnet build RightMove.sln --configuration Release /p:AssemblyVersion=1.1.1.$BUILD_NUMBER