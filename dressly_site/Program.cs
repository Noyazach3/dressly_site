var builder = WebApplication.CreateBuilder(args);

// ����� IConfiguration ��������
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ����� ������ ������ ���� appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing or empty.");
}
else
{
    Console.WriteLine($"Connection string: {connectionString}");
}

// ����� ClothingService �� ������ �����
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
