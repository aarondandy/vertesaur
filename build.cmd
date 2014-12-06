@echo OFF
pushd %~dp0build
scriptcs -install
scriptcs baufile.csx -- %*
popd