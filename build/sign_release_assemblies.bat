call "%VS110COMNTOOLS%..\..\VC\vcvarsall.bat" x86
sn.exe -R "%~dp0..\bin\Release\net40-client\Vertesaur.Core.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\net40-client\CodeContracts\Vertesaur.Core.Contracts.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\net40-client\Vertesaur.Generation.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\net40-client\CodeContracts\Vertesaur.Generation.Contracts.dll" "%~dp0..\vertesaur-keypair.snk"

sn.exe -R "%~dp0..\bin\Release\sl4\Vertesaur.Core.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\sl5\Vertesaur.Core.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\windows8\Vertesaur.Core.dll" "%~dp0..\vertesaur-keypair.snk"

sn.exe -R "%~dp0..\bin\Release\sl5\Vertesaur.Generation.dll" "%~dp0..\vertesaur-keypair.snk"
sn.exe -R "%~dp0..\bin\Release\windows8\Vertesaur.Generation.dll" "%~dp0..\vertesaur-keypair.snk"