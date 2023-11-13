#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Meridian.PlatformManagement/Meridian.PlatformManagement.csproj", "Meridian.PlatformManagement/"]
COPY ["Meridian.PlatformManagement.Core/Meridian.PlatformManagement.Core.csproj", "Meridian.PlatformManagement.Core/"]
COPY ["Meridian.PlatformManagement.Infrastructure/Meridian.PlatformManagement.Infrastructure.csproj", "Meridian.PlatformManagement.Infrastructure/"]
RUN dotnet restore "Meridian.PlatformManagement/Meridian.PlatformManagement.csproj"
COPY . .
WORKDIR "/src/Meridian.PlatformManagement"
RUN dotnet build "Meridian.PlatformManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meridian.PlatformManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meridian.PlatformManagement.dll"]