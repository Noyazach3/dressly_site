using dressly_site.Components;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// הוספת Razor Pages ו-Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// הגדרת HttpClient עם כתובת בסיסית ל-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/"); // כתובת ה-API שלך
});

// רישום ClothingService עם HttpClient
builder.Services.AddScoped(sp =>
    new ClothingService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
