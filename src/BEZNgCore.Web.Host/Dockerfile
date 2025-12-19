#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/BEZNgCore.Web.Host/BEZNgCore.Web.Host.csproj", "src/BEZNgCore.Web.Host/"]
COPY ["src/BEZNgCore.Web.Core/BEZNgCore.Web.Core.csproj", "src/BEZNgCore.Web.Core/"]
COPY ["src/BEZNgCore.Application/BEZNgCore.Application.csproj", "src/BEZNgCore.Application/"]
COPY ["src/BEZNgCore.Application.Shared/BEZNgCore.Application.Shared.csproj", "src/BEZNgCore.Application.Shared/"]
COPY ["src/BEZNgCore.Core.Shared/BEZNgCore.Core.Shared.csproj", "src/BEZNgCore.Core.Shared/"]
COPY ["src/BEZNgCore.Core/BEZNgCore.Core.csproj", "src/BEZNgCore.Core/"]
COPY ["src/BEZNgCore.EntityFrameworkCore/BEZNgCore.EntityFrameworkCore.csproj", "src/BEZNgCore.EntityFrameworkCore/"]
COPY ["src/BEZNgCore.GraphQL/BEZNgCore.GraphQL.csproj", "src/BEZNgCore.GraphQL/"]
RUN dotnet restore "src/BEZNgCore.Web.Host/BEZNgCore.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/BEZNgCore.Web.Host"
RUN dotnet build "BEZNgCore.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BEZNgCore.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BEZNgCore.Web.Host.dll"]
