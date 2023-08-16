FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Wheels-n-Deals.API/Wheels-n-Deals.API.csproj", "Wheels-n-Deals.API/"]
RUN dotnet restore "Wheels-n-Deals.API/Wheels-n-Deals.API.csproj"
COPY . .
WORKDIR "/src/Wheels-n-Deals.API"
RUN dotnet build "Wheels-n-Deals.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wheels-n-Deals.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wheels-n-Deals.API.dll"]