var builder = WebApplication.CreateBuilder(args);

// הוספת IConfiguration לשירותים
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// קריאת מחרוזת החיבור מתוך appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing or empty.");
}
else
{
    Console.WriteLine($"Connection string: {connectionString}");
}

// רישום ClothingService עם מחרוזת חיבור
builder.Services.AddScoped(sp => new ClothingService(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
