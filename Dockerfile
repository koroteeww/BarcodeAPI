#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["BarcodeAPI.csproj", "."]
COPY ["NuGet.config", "NuGet.config"]
RUN dotnet restore --configfile NuGet.config "./BarcodeAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BarcodeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BarcodeAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarcodeAPI.dll"]