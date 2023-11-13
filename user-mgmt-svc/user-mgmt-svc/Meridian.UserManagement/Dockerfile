#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Meridian.UserManagement/Meridian.UserManagement.csproj", "Meridian.UserManagement/"]
COPY ["Meridian.UserManagement.Core/Meridian.UserManagement.Core.csproj", "Meridian.UserManagement.Core/"]
COPY ["Meridian.UserManagement.Infrastructure/Meridian.UserManagement.Infrastructure.csproj", "Meridian.UserManagement.Infrastructure/"]
RUN dotnet restore "Meridian.UserManagement/Meridian.UserManagement.csproj"
COPY . .
WORKDIR "/src/Meridian.UserManagement"
RUN dotnet build "Meridian.UserManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meridian.UserManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meridian.UserManagement.dll"]