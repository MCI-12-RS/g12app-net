# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/G12App.API"
RUN dotnet build "G12App.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN apt-get update -y
RUN apt-get install npm -y
RUN dotnet publish "G12App.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "G12App.API.dll"]