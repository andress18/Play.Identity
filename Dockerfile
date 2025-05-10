FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Play.Identity.Service/Play.Identity.Service.csproj", "src/Play.Identity.Service/"]
COPY ["src/Play.Identity.Contracts/Play.Identity.Contracts.csproj", "src/Play.Identity.Contracts/"]

RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    dotnet nuget add source --username USERNAME --password `cat /GH_PAT` --store-password-in-clear-text --name github "https://nuget.pkg.github.com/`cat /GH_OWNER`/index.json"

RUN dotnet restore "src/Play.Identity.Service/Play.Identity.Service.csproj"
COPY . .
WORKDIR "/src/src/Play.Identity.Service"
RUN dotnet build "Play.Identity.Service.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Play.Identity.Service.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Play.Identity.Service.dll"]
