#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ControlCenterMockApi/ControlCenterMockApi.csproj", "ControlCenterMockApi/"]
ARG PAT=localhost
RUN sed -i "s|</configuration>|<packageSourceCredentials><local><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></local></packageSourceCredentials></configuration>|" nuget.config

RUN dotnet restore "ControlCenterMockApi/ControlCenterMockApi.csproj" --configfile "./nuget.config"
COPY . .
WORKDIR "/src/ControlCenterMockApi"
RUN dotnet build "ControlCenterMockApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ControlCenterMockApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ControlCenterMockApi.dll"]