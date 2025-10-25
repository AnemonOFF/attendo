# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY Attendo.WebAPI/Attendo.WebAPI.csproj Attendo.WebAPI/
COPY Attendo.Application/Attendo.Application.csproj Attendo.Application/
COPY Attendo.Domain/Attendo.Domain.csproj Attendo.Domain/
COPY Attendo.Infrastructure/Attendo.Infrastructure.csproj Attendo.Infrastructure/
COPY Attendo.Persistence/Attendo.Persistence.csproj Attendo.Persistence/
COPY Attendo.Shared/Attendo.Shared.csproj Attendo.Shared/

RUN dotnet restore Attendo.WebAPI/Attendo.WebAPI.csproj

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "Attendo.WebAPI/Attendo.WebAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Attendo.WebAPI/Attendo.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9.0 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "Attendo.WebAPI.dll"]

