# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CorporateSoccerWorldCup.Api/CorporateSoccerWorldCup.Api.csproj", "CorporateSoccerWorldCup.Api/"]
COPY ["CorporateSoccerWorldCup.Application/CorporateSoccerWorldCup.Application.csproj", "CorporateSoccerWorldCup.Application/"]
COPY ["CorporateSoccerWorldCup.Domain/CorporateSoccerWorldCup.Domain.csproj", "CorporateSoccerWorldCup.Domain/"]
COPY ["CorporateSoccerWorldCup.Infrastructure/CorporateSoccerWorldCup.Infrastructure.csproj", "CorporateSoccerWorldCup.Infrastructure/"]

RUN dotnet restore "CorporateSoccerWorldCup.Api/CorporateSoccerWorldCup.Api.csproj"

COPY . .
WORKDIR "/src/CorporateSoccerWorldCup.Api"
RUN dotnet publish "CorporateSoccerWorldCup.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CorporateSoccerWorldCup.Api.dll"]