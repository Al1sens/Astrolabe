using Astrolabe.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ШАГ 1: Регистрируем контроллеры
builder.Services.AddControllers();

// ШАГ 2: Берём строку подключения к Neon (PostgreSQL) из конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ШАГ 3: Подключаем слоника (Npgsql)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Astrolabe.Auth";
        options.Cookie.HttpOnly = true; // Защита: JS на странице не сможет украсть куку
        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Запоминаем вход на 30 дней
        options.SlidingExpiration = true; // Продлеваем срок, если пользователь заходит на сайт

        // Для API-запросов (fetch) отдаём код 401 вместо редиректа на страницу логина
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

// ШАГ 4: Собираем объект приложения (только после этого появляется переменная app!)
var app = builder.Build();

app.UseHttpsRedirection();

// ШАГ 5: Включаем раздачу нашего index.html из папки wwwroot (строго перед MapControllers!)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

// ШАГ 6: Подключаем маршруты API-контроллеров (/api/stars)
app.MapControllers();

// ШАГ 7: Автоматически накатываем миграции EF Core перед запуском сервера
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ШАГ 8: Запускаем веб-сервер (строго один раз в самом конце!)
app.Run();