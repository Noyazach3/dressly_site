namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // מוסיף את IConfiguration לשירותים
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

            // בדיקת מחרוזת החיבור
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string is missing or empty.");
            }
            else
            {
                Console.WriteLine($"Connection string: {connectionString}");
            }

            // הוספת ClothingService עם קריאת מחרוזת החיבור מתוך IConfiguration
            builder.Services.AddScoped<ClothingService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new ClothingService(connectionString);
            });

            // הוספת שירותים ל-API
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // הפעלת Swagger רק במצב פיתוח
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // הפעלת Static Files אם נדרש
            app.UseStaticFiles();

            // הפעלת Middleware
            app.UseRouting();
            app.UseCors(); // הפעלת CORS
            app.UseAuthorization();

            // מיפוי ה-Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
