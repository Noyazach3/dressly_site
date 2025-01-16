using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים לאפליקציה
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// הוספת HttpClient עם כתובת בסיס נכונה ל-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:40132/swagger/index.html");
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

// סדר הפעלת ה-Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
