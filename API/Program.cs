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
