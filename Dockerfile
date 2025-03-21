FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
COPY appsettings.Production.json /app/appsettings.Production.json

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

EXPOSE 8080
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet" , "DevKid.dll"]