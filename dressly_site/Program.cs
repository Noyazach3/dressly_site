using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// ����� IConfiguration ��������
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ����� ������ CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:57864") // ������ �� Blazor
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ����� ������ ������ �-appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ����� ClothingService �� ������ ������
builder.Services.AddScoped(_ => new ClothingService(connectionString));

// ����� ������� ���������
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ����� HttpClient �-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:40132/swagger/index.html"); // ����� ������ �-API
});

// ����� Middleware �� Antiforgery
builder.Services.AddAntiforgery();

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

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

// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(); // ����� CORS
app.UseAntiforgery();

// ����� �-Controllers
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
