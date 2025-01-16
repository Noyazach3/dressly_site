using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// ����� ������� ���������
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ����� HttpClient �� ����� ���� ����� �-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:40132/swagger/index.html");
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

// ��� ����� �-Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
