# Этап сборки приложения на .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем NuGet-пакеты
COPY *.csproj ./
RUN dotnet restore

# Копируем весь остальной код и публикуем релизную сборку
COPY . ./
RUN dotnet publish -c Release -o /app/publish

#  Лёгкий образ среды выполнения ASP.NET Core 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render сам пробрасывает порт через переменную PORT, по умолчанию ставим 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# ВАЖНО: Если твой .csproj называется иначе, замени Astrolabe.dll на <ИмяПроекта>.dll
ENTRYPOINT ["dotnet", "Astrolabe.dll"]