namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ����� �� IConfiguration ��������
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // ����� ������ CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:57864") // ������ �� Blazor
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ����� ������� �-API
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ����� Swagger �� ���� �����
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ����� Static Files �� ����
            app.UseStaticFiles();

            // ����� Middleware
            app.UseRouting();
            app.UseCors(); // ����� CORS
            app.UseAuthorization();

            // ����� �-Controllers
            app.MapControllers();

            app.Run();
        }
    }
}