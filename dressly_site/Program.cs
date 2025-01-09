using Microsoft.EntityFrameworkCore;
using dressly_site.Data;
using dressly_site.Models;
using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים לאפליקציה
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// הוספת HttpClient אם צריך לתקשורת עם ה-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:57864/");
});

// הוספת ה-DbContext עם חיבור למסד נתונים MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 28)))); // שינוי גרסה בהתאם לגרסה שלך

// הוספת שירותים לבקרים (Controllers)
builder.Services.AddControllers();

// הוספת Swagger לשירותים אם במצב פיתוח
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// הפעלת Swagger במצב פיתוח
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

// אם יש צורך ב-AntiForgery (כמו בטפסים)
app.UseAntiforgery();

// מיפוי Controllers
app.MapControllers();

// מיפוי Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
