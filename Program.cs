using Astrolabe.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ШАГ 1: Регистрируем контроллеры
builder.Services.AddControllers();

// ШАГ 2: Берём строку подключения к Neon (PostgreSQL) из конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ШАГ 3: Подключаем слоника (Npgsql)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

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