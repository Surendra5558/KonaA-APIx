# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY KonaAI.Master/KonaAI.Master.API/KonaAI.Master.API.csproj KonaAI.Master.API/
COPY KonaAI.Master/KonaAI.Master.Business/KonaAI.Master.Business.csproj KonaAI.Master.Business/
COPY KonaAI.Master/KonaAI.Master.Model/KonaAI.Master.Model.csproj KonaAI.Master.Model/
COPY KonaAI.Master/KonaAI.Master.Repository/KonaAI.Master.Repository.csproj KonaAI.Master.Repository/

# Restore dependencies
RUN dotnet restore KonaAI.Master.API/KonaAI.Master.API.csproj

# Copy rest of the source code
COPY KonaAI.Master .

# Publish the application
RUN dotnet publish KonaAI.Master.API/KonaAI.Master.API.csproj -c $BUILD_CONFIGURATION -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0.7 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app .

# Set the entry point
ENTRYPOINT ["dotnet", "KonaAI.Master.API.dll"]

# Expose ports
EXPOSE 8080
EXPOSE 8081
