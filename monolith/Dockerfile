# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Optivem.AtddAccelerator.EShop.Monolith.csproj", "./"]
RUN dotnet restore "Optivem.AtddAccelerator.EShop.Monolith.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "Optivem.AtddAccelerator.EShop.Monolith.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Optivem.AtddAccelerator.EShop.Monolith.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Optivem.AtddAccelerator.EShop.Monolith.dll"]
