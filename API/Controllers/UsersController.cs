using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;

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
    public IActionResult ValidateLogin([FromQuery] string username, [FromQuery] string passwordHash, [FromQuery] string email)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // שאילתה לבדיקה אם המשתמש קיים
                string query = "SELECT COUNT(*) FROM users WHERE Username = @Username AND PasswordHash = @PasswordHash AND Email = @Email";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // הוספת פרמטרים לשאילתה
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@Email", email);

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

    // פעולה לרישום משתמש חדש
    [HttpPost("Register")]
    public IActionResult Register([FromBody] User newUser)
    {
        if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.PasswordHash) || string.IsNullOrWhiteSpace(newUser.Email))
        {
            return BadRequest("All fields are required.");
        }

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // בדיקה אם המשתמש כבר קיים
                string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @Username";

                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", newUser.Username);
                    int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userCount > 0)
                    {
                        return BadRequest("User already exists.");
                    }
                }

                // הוספת המשתמש החדש
                string insertQuery = "INSERT INTO users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email)";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", newUser.Username);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                    insertCmd.Parameters.AddWithValue("@Email", newUser.Email);

                    insertCmd.ExecuteNonQuery();
                }

                return Ok("User registered successfully.");
            }
        }
        catch (Exception ex)
        {
            // טיפול בשגיאות
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }
}

// מודל עבור המשתמש
public class User
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
}
