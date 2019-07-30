# Create the build environment image
FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0 as build-env
WORKDIR /app
 
# Copy the project file and restore the dependencies
COPY contracts/*.csproj ./contracts/
COPY client/*.csproj ./client/

RUN dotnet restore client/client.csproj
RUN dotnet restore contracts/contracts.csproj

# Copy the remaining source files and build the application
COPY . ./
RUN dotnet publish client/client.csproj -c Release -o out
 
# Build the runtime image
#FROM mcr.microsoft.com/dotnet/core-nightly/runtime:3.0
FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.0
WORKDIR /app
COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "--list-runtimes"]
ENTRYPOINT ["dotnet", "client.dll", "10000"]