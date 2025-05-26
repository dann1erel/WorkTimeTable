using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();
app.MapRazorPages();
app.Run();