using Microsoft.EntityFrameworkCore;
using dressly_site.Data;
using dressly_site.Models;
using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// ����� ������� ���������
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ����� HttpClient �� ���� ������� �� �-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:57864/");
});

// ����� �-DbContext �� ����� ���� ������ MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 28)))); // ����� ���� ����� ����� ���

// ����� ������� ������ (Controllers)
builder.Services.AddControllers();

// ����� Swagger �������� �� ���� �����
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// ����� Swagger ���� �����
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

// �� �� ���� �-AntiForgery (��� ������)
app.UseAntiforgery();

// ����� Controllers
app.MapControllers();

// ����� Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
