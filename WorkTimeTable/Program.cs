using Microsoft.EntityFrameworkCore;
using WorkTimeTable.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();
app.MapRazorPages();
app.Run();