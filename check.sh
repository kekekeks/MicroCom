#!/bin/bash
dotnet pack
cd TestApp
dotnet msbuild /nr:false /t:Unpack
dotnet msbuild /nr:false
