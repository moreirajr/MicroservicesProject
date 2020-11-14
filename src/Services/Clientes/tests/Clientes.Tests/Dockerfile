#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Services/Clientes/tests/Clientes.Tests/Clientes.Tests.csproj", "src/Services/Clientes/tests/Clientes.Tests/"]
COPY ["src/Services/Clientes/Clientes.Api/Clientes.Api.csproj", "src/Services/Clientes/Clientes.Api/"]
COPY ["src/Services/Clientes/Clientes.Application/Clientes.Application.csproj", "src/Services/Clientes/Clientes.Application/"]
COPY ["src/Services/Clientes/Clientes.Domain/Clientes.Domain.csproj", "src/Services/Clientes/Clientes.Domain/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Core/EventBus.Core.csproj", "src/BuildingBlocks/EventBus/EventBus.Core/"]
COPY ["src/Services/Clientes/Clientes.Infra.CrossCutting.IoC/Clientes.Infra.CrossCutting.IoC.csproj", "src/Services/Clientes/Clientes.Infra.CrossCutting.IoC/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBus.RabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/Services/Clientes/Clientes.Infra.Data/Clientes.Infra.Data.csproj", "src/Services/Clientes/Clientes.Infra.Data/"]
RUN dotnet restore "src/Services/Clientes/tests/Clientes.Tests/Clientes.Tests.csproj"
COPY . .
WORKDIR "/src/src/Services/Clientes/tests/Clientes.Tests"
RUN dotnet build "Clientes.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Clientes.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clientes.Tests.dll"]