FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN dotnet tool install --global dotnet-ef

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Wheels-n-Deals.API.dll"]