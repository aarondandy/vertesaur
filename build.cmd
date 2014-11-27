@echo OFF
pushd %~dp0build
scriptcs baufile.csx -- %*
popd