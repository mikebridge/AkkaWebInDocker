# Akka.Net Remote / WebApi / .NET Core 2.0 Example

Basic setup for Akka.Net 

TODO: Add the docker configuration

## Build - Visual Studio

You can build and run this without docker from Visual Studio. 

From the command line, you can run this in docker using Linux containers:

```powershell
> dotnet build
> dotnet publish
> docker-compose up -build
```

You should then be able to send a GET request to [http://localhost:3000/api/echo/test](http://localhost:3000/api/echo/test).

## Notes

- Make sure you have EchoConsoleApp -> Build -> Advanced Settings... -> Language version set to "C# latest minor version (latest)" so that it uses the `HostBuilder` which is only available from C# 7.1.

botht 
