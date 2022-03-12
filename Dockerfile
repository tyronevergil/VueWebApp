FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY Persistence/Persistence.csproj Persistence/
COPY SimpleBus/SimpleBus.csproj SimpleBus/
COPY WebApplication/WebApplication.csproj WebApplication/
RUN dotnet restore "WebApplication/WebApplication.csproj"
COPY . .
WORKDIR "/src/WebApplication"
RUN dotnet build "WebApplication.csproj" -c Release -o /app/build

FROM build AS publish
# Install NodeJs
RUN apt-get update && \
apt-get install -y wget && \
apt-get install -y gnupg2 && \
wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
apt-get install -y build-essential nodejs
# End Install
RUN dotnet publish "WebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:$PORT
ENTRYPOINT ["dotnet", "WebApplication.dll"]
