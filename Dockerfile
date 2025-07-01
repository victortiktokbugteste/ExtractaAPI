FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IntegracaoAngular/IntegracaoAngular.csproj", "IntegracaoAngular/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "IntegracaoAngular/IntegracaoAngular.csproj"
COPY . .
WORKDIR "/src/IntegracaoAngular"
RUN dotnet build "IntegracaoAngular.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IntegracaoAngular.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IntegracaoAngular.dll"] 