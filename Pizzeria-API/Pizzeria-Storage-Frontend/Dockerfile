#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Pizzeria-Storage-Frontend/Pizzeria-Storage-Frontend.csproj", "Pizzeria-Storage-Frontend/"]
RUN dotnet restore "Pizzeria-Storage-Frontend/Pizzeria-Storage-Frontend.csproj"
COPY . .
WORKDIR "/src/Pizzeria-Storage-Frontend"
RUN dotnet build "Pizzeria-Storage-Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pizzeria-Storage-Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pizzeria-Storage-Frontend.dll"]