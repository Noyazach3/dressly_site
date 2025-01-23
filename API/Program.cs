using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // הוספת IConfiguration לשירותים
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // קריאת מחרוזת החיבור
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

            // הוספת שירותים ל-API
            builder.Services.AddControllers();

            // הוספת Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            // מיפוי ה-Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
