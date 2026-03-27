# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all source files and publish the app
COPY . ./
RUN dotnet publish -c Release -o out

# -------------------------
# Runtime stage
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out ./

# Expose application port
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "DotnetMongoDocker.dll"]