# Create the build environment image
FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0 as build-env
WORKDIR /app
 
# Copy the project file and restore the dependencies
#COPY . ./
COPY contracts/*.csproj ./contracts/
COPY server/*.csproj ./server/
#RUN ls server
RUN dotnet restore server/server.csproj
RUN dotnet restore contracts/contracts.csproj
#RUN dotnet restore

# Copy the remaining source files and build the application
COPY . ./
RUN dotnet publish server/server.csproj -c Release -o out
 
# Build the runtime image
#FROM mcr.microsoft.com/dotnet/core-nightly/runtime:3.0
FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0
WORKDIR /app
COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "--list-runtimes"]
ENTRYPOINT ["dotnet", "--fx-version", "3.0.0-preview7.19365.7", "server.dll"]