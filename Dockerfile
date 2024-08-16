# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["G12App.API/G12App.API.csproj", "G12App.API/"]
RUN dotnet restore "G12App.API/G12App.API.csproj"
COPY . .
WORKDIR "/src/G12App.API"
RUN dotnet build "G12App.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "G12App.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "G12App.API.dll"]