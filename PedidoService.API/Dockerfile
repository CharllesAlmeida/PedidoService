FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PedidoService.API/PedidoService.API.csproj", "PedidoService.API/"]
COPY ["PedidoService.Application/PedidoService.Application.csproj", "PedidoService.Application/"]
COPY ["PedidoService.Domain/PedidoService.Domain.csproj", "PedidoService.Domain/"]
COPY ["PedidoService.Infrastructure/PedidoService.Infrastructure.csproj", "PedidoService.Infrastructure/"]

RUN dotnet restore "PedidoService.API/PedidoService.API.csproj"

COPY . .

WORKDIR "/src/PedidoService.API"
RUN dotnet build "PedidoService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PedidoService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

VOLUME ["/root/.aspnet/https/", "/root/.microsoft/usersecrets"]

ENTRYPOINT ["dotnet", "PedidoService.API.dll"]
