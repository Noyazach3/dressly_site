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

            // ����� IConfiguration ��������
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // ����� ������ ������
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

            // ����� ������� �-API
            builder.Services.AddControllers();

            // ����� Swagger
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

            // ����� �-Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
