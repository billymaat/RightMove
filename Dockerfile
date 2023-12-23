FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /RM

# Copy everything
COPY . ./

WORKDIR /RM/RightMoveConsole

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /RM/RightMoveConsole/out
COPY --from=build-env /RM/RightMoveConsole/out .
ENV SomeVar="Testing a string"
ENTRYPOINT ["dotnet", "RightMoveConsole.dll"]