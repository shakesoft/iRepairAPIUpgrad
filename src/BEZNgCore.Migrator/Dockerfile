#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/BEZNgCore.Migrator/BEZNgCore.Migrator.csproj", "src/BEZNgCore.Migrator/"]
COPY ["src/BEZNgCore.EntityFrameworkCore/BEZNgCore.EntityFrameworkCore.csproj", "src/BEZNgCore.EntityFrameworkCore/"]
COPY ["src/BEZNgCore.Core/BEZNgCore.Core.csproj", "src/BEZNgCore.Core/"]
COPY ["src/BEZNgCore.Core.Shared/BEZNgCore.Core.Shared.csproj", "src/BEZNgCore.Core.Shared/"]
RUN dotnet restore "src/BEZNgCore.Migrator/BEZNgCore.Migrator.csproj"
COPY . .
WORKDIR "/src/src/BEZNgCore.Migrator"
RUN dotnet build "BEZNgCore.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BEZNgCore.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BEZNgCore.Migrator.dll", "-s"]
