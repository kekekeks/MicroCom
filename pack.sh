#!/bin/bash
set -x
set -e
find .|grep nupkg|xargs rm
rm -rf artifacts
mkdir artifacts
dotnet pack -c Release
cp `find . | grep nupkg` artifacts

