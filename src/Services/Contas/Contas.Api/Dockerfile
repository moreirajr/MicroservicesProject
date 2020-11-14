#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Services/Contas/Contas.Api/Contas.Api.csproj", "src/Services/Contas/Contas.Api/"]
COPY ["src/Services/Contas/Contas.Infra.CrossCutting.IoC/Contas.Infra.CrossCutting.IoC.csproj", "src/Services/Contas/Contas.Infra.CrossCutting.IoC/"]
COPY ["src/Services/Contas/Contas.Infra.CrossCutting/Contas.Infra.CrossCutting.csproj", "src/Services/Contas/Contas.Infra.CrossCutting/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBus.RabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Core/EventBus.Core.csproj", "src/BuildingBlocks/EventBus/EventBus.Core/"]
COPY ["src/Services/Contas/Contas.Application/Contas.Application.csproj", "src/Services/Contas/Contas.Application/"]
COPY ["src/Services/Contas/Contas.Domain/Contas.Domain.csproj", "src/Services/Contas/Contas.Domain/"]
COPY ["src/Services/Contas/Contas.Infra.Data/Contas.Infra.Data.csproj", "src/Services/Contas/Contas.Infra.Data/"]
RUN dotnet restore "src/Services/Contas/Contas.Api/Contas.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Contas/Contas.Api"
RUN dotnet build "Contas.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contas.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contas.Api.dll"]