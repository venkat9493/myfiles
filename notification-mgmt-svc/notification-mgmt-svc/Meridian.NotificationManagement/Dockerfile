#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Meridian.NotificationManagement/Meridian.NotificationManagement.csproj", "Meridian.NotificationManagement/"]
COPY ["Meridian.NotificationManagement.Core/Meridian.NotificationManagement.Core.csproj", "Meridian.NotificationManagement.Core/"]
COPY ["Meridian.NotificationManagement.Infrastructure/Meridian.NotificationManagement.Infrastructure.csproj", "Meridian.NotificationManagement.Infrastructure/"]
RUN dotnet restore "Meridian.NotificationManagement/Meridian.NotificationManagement.csproj"
COPY . .
WORKDIR "/src/Meridian.NotificationManagement"
RUN dotnet build "Meridian.NotificationManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meridian.NotificationManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meridian.NotificationManagement.dll"]