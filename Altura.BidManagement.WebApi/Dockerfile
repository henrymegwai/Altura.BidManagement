FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Altura.BidManagement.WebApi/Altura.BidManagement.WebApi.csproj", "Altura.BidManagement.WebApi/"]

RUN dotnet restore "Altura.BidManagement.WebApi/Altura.BidManagement.WebApi.csproj"
COPY . .

WORKDIR "/src/Altura.BidManagement.WebApi"
RUN dotnet build "Altura.BidManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Altura.BidManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Altura.BidManagement.WebApi.dll"]
