# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file
COPY ["src.sln", "./"]

# Copy all project files to restore dependencies
COPY ["Core/Attendo.Application/Attendo.Application.csproj", "Core/Attendo.Application/"]
COPY ["Core/Attendo.Domain/Attendo.Domain.csproj", "Core/Attendo.Domain/"]
COPY ["Infrastructure/Attendo.Infrastructure/Attendo.Infrastructure.csproj", "Infrastructure/Attendo.Infrastructure/"]
COPY ["Infrastructure/Attendo.Persistence/Attendo.Persistence.csproj", "Infrastructure/Attendo.Persistence/"]
COPY ["Presentation/Attendo.WebAPI/Attendo.WebAPI.csproj", "Presentation/Attendo.WebAPI/"]
COPY ["Shared/Attendo.Shared/Attendo.Shared.csproj", "Shared/Attendo.Shared/"]

# Restore dependencies
RUN dotnet restore "src.sln"

# Copy the rest of the source code
COPY . .

# Build the solution
RUN dotnet build "src.sln" -c Release -o /app/build

# Publish the WebAPI project
FROM build AS publish
RUN dotnet publish "Presentation/Attendo.WebAPI/Attendo.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 9.0 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published application
COPY --from=publish /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "Attendo.WebAPI.dll"]
