FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CQRSMediatr.API/CQRSMediatr.API.csproj", "CQRSMediatr.API/"]
COPY ["CQRSMediatr.DataService/CQRSMediatr.DataService.csproj", "CQRSMediatr.DataService/"]
COPY ["CQRSMediatr.Entities/CQRSMediatr.Entities.csproj", "CQRSMediatr.Entities/"]
RUN dotnet restore "CQRSMediatr.API/CQRSMediatr.API.csproj"
## copy everything from local to docker image
COPY . .
WORKDIR "/src/CQRSMediatr.API"
RUN dotnet build "CQRSMediatr.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

## whatever taken from this build, give different name as publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "CQRSMediatr.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "CQRSMediatr.API.dll"]