using ClassLibrary1.Dtos;
using dressly_site.Components;

namespace dressly_site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // הוספת LoginSession לשירותים
            builder.Services.AddSingleton<UserDto>();

            // Register HttpClient עם כתובת בסיס
            builder.Services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri("http://localhost:40132/api/");
            });

            // הוספת שירותי Controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            // הוספת Middleware עבור Authorization
            app.UseRouting();
            app.UseAuthentication(); // אם יש לך Authentication
            app.UseAuthorization(); // קריטי להפעיל את המדיניות

            app.UseAntiforgery();

            // רישום נתיבים עבור API
            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
