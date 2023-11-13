#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN mkdir /root/.tiledb
RUN apt-get update -y && apt-get install python3 -y && apt-get install python3-distutils -y
WORKDIR /app
EXPOSE 80
EXPOSE 443

#start - python install
RUN apt-get install -y curl
RUN curl -k https://bootstrap.pypa.io/get-pip.py -o get-pip.py
RUN python3 get-pip.py
RUN pip install --upgrade pip 
RUN pip install tiledb && pip install tiledb-cloud
COPY . .
#end - python install

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Meridian.CatalogManagement/Meridian.CatalogManagement.csproj", "Meridian.CatalogManagement/"]
COPY ["Meridian.CatalogManagement.Infrastructure/Meridian.CatalogManagement.Infrastructure.csproj", "Meridian.CatalogManagement.Infrastructure/"]
COPY ["Meridian.CatalogManagement.Core/Meridian.CatalogManagement.Core.csproj", "Meridian.CatalogManagement.Core/"]
RUN dotnet restore "Meridian.CatalogManagement/Meridian.CatalogManagement.csproj"
COPY . .
WORKDIR "/src/Meridian.CatalogManagement"
RUN dotnet build "Meridian.CatalogManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meridian.CatalogManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meridian.CatalogManagement.dll"]