using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// הוספת IConfiguration לשירותים
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// הוספת שירותי CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:57864") // הכתובת של Blazor
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// קריאת מחרוזת החיבור מ-appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// הוספת ClothingService עם מחרוזת החיבור
builder.Services.AddScoped(_ => new ClothingService(connectionString));

// הוספת שירותים לאפליקציה
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// הוספת HttpClient ל-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:40132/swagger/index.html"); // כתובת בסיסית ל-API
});

// הוספת Middleware של Antiforgery
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
app.UseCors(); // הפעלת CORS
app.UseAntiforgery();

// מיפוי ה-Controllers
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
