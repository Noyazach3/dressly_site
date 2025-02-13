using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "AdminOnly")] // רק למנהלים
public class AdminController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AdminController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 🔹 שליפת כל המשתמשים במערכת
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        List<UserInfo> users = new();
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Username, Email, Role FROM Users";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new UserInfo
                        {
                            Username = reader.GetString("Username"),
                            Email = reader.GetString("Email"),
                            Role = reader.GetString("Role")
                        });
                    }
                }
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "שגיאה בעת שליפת המשתמשים", Error = ex.Message });
        }
    }

    // 🔹 מחיקת משתמש לפי שם משתמש
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser([FromQuery] string username)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Users WHERE Username = @Username";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                        return Ok("המשתמש נמחק בהצלחה");
                    else
                        return NotFound("המשתמש לא נמצא");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "שגיאה בעת מחיקת המשתמש", Error = ex.Message });
        }
    }

    // 🔹 איפוס סיסמה של משתמש (קובעת סיסמה ברירת מחדל)
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromQuery] string username)
    {
        string defaultPassword = "123456"; // סיסמה ברירת מחדל
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Username = @Username";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", defaultPassword);
                    command.Parameters.AddWithValue("@Username", username);

                    int affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                        return Ok("סיסמת המשתמש אופסה בהצלחה");
                    else
                        return NotFound("המשתמש לא נמצא");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "שגיאה בעת איפוס הסיסמה", Error = ex.Message });
        }
    }

    // 🔹 שליפת סטטיסטיקות האתר
    [HttpGet("GetStatistics")]
    public async Task<IActionResult> GetStatistics()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        StatisticsResponse stats = new();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // מספר המשתמשים
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Users", connection))
                    stats.TotalUsers = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                // מספר הבגדים שהועלו
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM ClothingItems", connection))
                    stats.TotalClothingItems = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                // הקטגוריות הפופולריות ביותר
                string categoryQuery = "SELECT Category, COUNT(*) AS Count FROM ClothingItems GROUP BY Category ORDER BY Count DESC LIMIT 5";
                using (var cmd = new MySqlCommand(categoryQuery, connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stats.PopularCategories.Add(new CategoryStat
                        {
                            Name = reader.GetString("Category"),
                            Count = reader.GetInt32("Count")
                        });
                    }
                }
            }
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "שגיאה בעת שליפת הסטטיסטיקות", Error = ex.Message });
        }
    }

    // 🔹 מחלקות נתונים להעברת המידע ל-Frontend
    public class UserInfo
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class CategoryStat
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class StatisticsResponse
    {
        public int TotalUsers { get; set; }
        public int TotalClothingItems { get; set; }
        public List<CategoryStat> PopularCategories { get; set; } = new();
    }
}
