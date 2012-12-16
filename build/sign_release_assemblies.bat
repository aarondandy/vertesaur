call "%VS110COMNTOOLS%..\..\VC\vcvarsall.bat" x86
sn.exe -R "%~dp0..\bin\Release\net40-client\Vertesaur.Core.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\net40-client\Vertesaur.Generation.dll" "%~dp0..\vertesaur-keypair.snk"