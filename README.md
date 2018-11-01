# Akka.Net Remote / WebApi / .NET Core 2.0 Example

Basic setup demo for Akka.Net in Docker.  

This is configured as two Linux containers, one running a Console app with an Akka.Net ActorSystem, and the other running WebApi.  The web api accesses
the Console app via Akka.Remote. The EchoConsole app has a single EchoActor.

## Build - Visual Studio w/o Docker

You can build and run both processes without docker from Visual Studio 2017 using `Debug -> Start Debugging`.  

## Build - Linux/Mac/Windows & Docker

_Prerequisites:_

- [.NET Core sdk](https://www.microsoft.com/net/download) 
- [Docker](https://docs.docker.com/)

You can build the app from the command line, build the docker containers from the published source, then launch the two processes together:

```powershell
> dotnet build
> dotnet publish
> docker-compose up -build
```

If this worked correctly, you should then be able to send a GET request to [http://localhost:3000/api/echo/test](http://localhost:3000/api/echo/test).  Right now 
the EchoActor does nothing but write to the debug console.

## Notes

- EchoConsoleApp -> Build -> Advanced Settings... -> Language version is set to "C# latest minor version (latest)" so 
that [HostBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=aspnetcore-2.1) is available
(requires C# 7.1).

- Currently there may be some [problems with  .NET Core 2.1](https://github.com/akkadotnet/akka.net/issues/3506)