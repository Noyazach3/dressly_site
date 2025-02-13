using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
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

    // ------------------------------------------------------------------
    // פעולה לאימות משתמש (ValidateLogin)
    // בודקת אם המשתמש קיים בטבלת users ואם פרטי ההתחברות נכונים.
    // מחזירה Ok(true) אם המשתמש נמצא, אחרת Ok(false).
    // ------------------------------------------------------------------
    [HttpGet("ValidateLogin")]
    public IActionResult ValidateLogin([FromQuery] string username, [FromQuery] string passwordHash)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM users WHERE Username = @Username AND PasswordHash = @PasswordHash";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                    return Ok(userCount > 0);
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }

    // ------------------------------------------------------------------
    // פעולה לרישום משתמש חדש (Register)
    // כל משתמש חדש מקבל תפקיד "User" כברירת מחדל.
    // ------------------------------------------------------------------
    [HttpPost("Register")]
    public IActionResult Register([FromBody] User newUser)
    {
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
                        return BadRequest("User already exists");
                    }
                }

                // הוספת המשתמש החדש עם תפקיד User
                string insertQuery = "INSERT INTO users (Username, PasswordHash, Email, Role) VALUES (@Username, @PasswordHash, @Email, 'User')";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", newUser.Username);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                    insertCmd.Parameters.AddWithValue("@Email", newUser.Email);

                    insertCmd.ExecuteNonQuery();
                }

                return Ok("User registered successfully");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }

    // ------------------------------------------------------------------
    // פעולה לקבלת תפקיד המשתמש
    // מחזירה את התפקיד של המשתמש: "User" או "Admin".
    // ------------------------------------------------------------------
    [HttpGet("GetUserRole")]
    public IActionResult GetUserRole([FromQuery] string username)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Role FROM users WHERE Username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var role = cmd.ExecuteScalar();

                    if (role != null)
                    {
                        return Ok(role.ToString());
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
        }
    }

    // ------------------------------------------------------------------
    // פעולה למחיקת משתמש (Admin בלבד)
    // מחקה משתמש לפי שם המשתמש. דורש הרשאת מנהל.
    // ------------------------------------------------------------------
    [HttpDelete("DeleteUser")]
    [Authorize(Policy = "AdminOnly")] // פעולה זו זמינה רק למנהלים
    public IActionResult DeleteUser([FromQuery] string username)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM users WHERE Username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User deleted successfully");
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
            }
        }
        catch (Exception ex)
        {
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
