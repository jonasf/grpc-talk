@rem Generate the C# code for .proto files

setlocal

set TOOLS_PATH="C:\Users\%USERNAME%\.nuget\packages\grpc.tools\1.9.0\tools\windows_x64"

%TOOLS_PATH%\protoc.exe --proto_path=..\..\protos --csharp_out Starwars  ..\..\protos\StarwarsService.proto --grpc_out Starwars --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

endlocal
