# ШАГ 1: Этап сборки приложения на .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем NuGet-пакеты
COPY Astrolabe.csproj ./
RUN dotnet restore Astrolabe.csproj

# Копируем весь остальной код и публикуем релизную сборку
COPY . ./
RUN dotnet publish Astrolabe.csproj -c Release -o /app/publish

# ШАГ 2: Лёгкий образ среды выполнения ASP.NET Core 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render сам пробрасывает порт, по умолчанию ставим 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Astrolabe.dll"]