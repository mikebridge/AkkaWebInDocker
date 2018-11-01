# MB: Should this be aspnetcore-build? https://github.com/aspnet/aspnet-docker/issues/322
#FROM microsoft/aspnetcore

FROM microsoft/aspnetcore-build

ENV ACCEPT_EULA Y
ENV ASPNETCORE_ENVIRONMENT DockerDev


ARG source=./bin/Debug/netcoreapp2.0/publish

WORKDIR /app
EXPOSE 5000 8082
COPY $source .

ENTRYPOINT ["dotnet", "Web.dll"]

