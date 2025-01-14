using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient; // חיבור ל-MySQL
using System.Data;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // פעולה לאימות משתמש
    [HttpGet("ValidateLogin")]
    public IActionResult ValidateLogin([FromQuery] string username, [FromQuery] string passwordHash)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // שאילתה לבדיקה אם המשתמש קיים
                string query = "SELECT COUNT(*) FROM users WHERE Username = @Username AND PasswordHash = @PasswordHash";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // הוספת פרמטרים לשאילתה
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    // הרצת השאילתה וקבלת התוצאה
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // החזרת true אם המשתמש קיים, אחרת false
                    return Ok(userCount > 0);
                }
            }
        }
        catch (Exception ex)
        {
            // טיפול בשגיאות
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }
}
