Vertesaur
=========

A computational geometry and math library for the CLR with potential medical, gaming, and GIS uses.

# About #

Vertesaur is the accumulation of the computational geometry algorithms and data structures required to support current and future projects. The data structures are intended to be reasonably independent of any one specific industry yet useful to many. The library is designed to run on the CLR and with .NET 4.0. Because this library is currently designed to support new projects there is little reason to put effort into supporting older versions of the framework and CLR.

## Features ##

All managed code with a goal of being portable across many platforms including .NET 4 (client), Mono, Silverlight 4-5, Windows Phone 7.1-8, and Windows 8.

# Building & Debugging #

The software can be built by building the solution in either Visual Studio or in MSBuild. A rake task is included that is capable of producing a release build of the project, provided you have the full key pair (you should not). **Before you can debug or execute** the result of a build on a development machine you must add an exception for assembly verification as the assemblies are delay signed. To disable assembly veriication for Vertesaur's public key you can run the included batch `./build/vertesaur_signing_validation_disable.bat` as Administrator at your own risk.