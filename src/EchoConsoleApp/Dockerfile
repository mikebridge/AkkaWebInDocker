# SEE also: https://github.com/petabridge/akkadotnet-code-samples/blob/master/Cluster.WebCrawler/src/WebCrawler.CrawlService/Dockerfile

FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

# These aren't essential, but may help with debugging.
RUN \
    apt-get update && \
	apt-get upgrade -y && \
    apt-get install -y software-properties-common \
                       apt-transport-https \
                       build-essential \
                       ca-certificates \
                       checkinstall \
                       curl \
                       emacs-nox \
                       git \
                       libssl-dev \
                       net-tools \
					   netcat \
                       openssh-client \
                       libffi-dev \
                       lftp \
                       rsync \
                       unzip \
					   telnet \
                       wget

ARG source=./bin/Debug/netcoreapp2.0/publish

EXPOSE 5002
COPY $source .

#ENV ASPNETCORE_ENVIRONMENT DockerDev
ENTRYPOINT ["dotnet", "EchoConsoleApp.dll"]
