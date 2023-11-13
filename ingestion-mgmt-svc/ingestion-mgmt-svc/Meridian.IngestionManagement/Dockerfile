#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Meridian.IngestionManagement/Meridian.IngestionManagement.csproj", "Meridian.IngestionManagement/"]
COPY ["Meridian.IngestionManagement.Infrastructure/Meridian.IngestionManagement.Infrastructure.csproj", "Meridian.IngestionManagement.Infrastructure/"]
COPY ["Meridian.IngestionManagement.Core/Meridian.IngestionManagement.Core.csproj", "Meridian.IngestionManagement.Core/"]
RUN dotnet restore "Meridian.IngestionManagement/Meridian.IngestionManagement.csproj"
COPY . .
WORKDIR "/src/Meridian.IngestionManagement"
RUN dotnet build "Meridian.IngestionManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meridian.IngestionManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meridian.IngestionManagement.dll","--environment=X"]