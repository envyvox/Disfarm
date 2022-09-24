FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Copy everything else and build
COPY Disfarm ./Disfarm
COPY Disfarm.Data ./Disfarm.Data
COPY Disfarm.Services ./Disfarm.Services

RUN dotnet restore Disfarm/Disfarm.csproj
RUN dotnet publish Disfarm/*.csproj -c Release -o out

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env ./out .
ENTRYPOINT ["dotnet", "Disfarm.dll"]