using dressly_site.Components;

var builder = WebApplication.CreateBuilder(args);

// äåñôú ùéøåúéí ìàôìé÷öéä
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// äåñôú HttpClient òí ëúåáú áñéñ ðëåðä ì-API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:40132/swagger/index.html");
});


// äåñôú Middleware ùì Antiforgery
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

// ñãø äôòìú ä-Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();