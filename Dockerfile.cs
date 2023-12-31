﻿# Используйте официальный образ ASP.NET Core
FROM mcr.microsoft.com / dotnet / aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApplication3.csproj", ""]
RUN dotnet restore "./WebApplication3.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApplication3.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "WebApplication3.csproj" -c Release -o /app/publish

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication3.dll"]