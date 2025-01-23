using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary1.Models; // הפניה למחלקות מתוך ClassLibrary1.Models

public class ClothingService
{
    private readonly string _connectionString;

    private readonly HttpClient _httpClient;

    public ClothingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }




    // פונקציה לספירת פריטים בארון
    public async Task<int> GetTotalItemsAsync(int userId)
    {
        int totalItems = 0;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "SELECT COUNT(*) FROM clothingitems WHERE UserID = @UserId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                var result = await command.ExecuteScalarAsync();
                totalItems = Convert.ToInt32(result);
            }
        }

        return totalItems;
    }

    // פונקציה לשליפת פריטים שדורשים כביסה
    public async Task<List<ClothingItem>> GetItemsForLaundryAsync(int userId)
    {
        var items = new List<ClothingItem>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "SELECT * FROM clothingitems WHERE UserID = @UserId AND TimesWorn >= WashAfterUses";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new ClothingItem
                        {
                            ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")), // int
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")), // int
                            Category = reader.GetString(reader.GetOrdinal("Category")), // varchar
                            ColorID = reader.GetInt32(reader.GetOrdinal("ColorID")), // int
                            Season = reader.GetString(reader.GetOrdinal("Season")), // varchar
                            ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")), // varchar
                            DateAdded = reader.IsDBNull(reader.GetOrdinal("DateAdded"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("DateAdded")), // date
                            LastWornDate = reader.IsDBNull(reader.GetOrdinal("LastWornDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("LastWornDate")), // date
                            WashAfterUses = reader.GetInt32(reader.GetOrdinal("WashAfterUses")), // int
                                                                                                 // הוספת UsageType ו-ColorName
                            UsageType = reader.GetString(reader.GetOrdinal("UsageType")), // varchar
                            ColorName = reader.GetString(reader.GetOrdinal("ColorName")) // varchar
                        });
                    }
                }
            }
        }

        return items;
    }



    // פונקציה להוספת פריט חדש
    public async Task AddClothingItemAsync(ClothingItem item)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
            INSERT INTO clothingitems (UserID, Category, ColorID, Season, ImageURL, DateAdded, WashAfterUses, LastWornDate)
            VALUES (@UserID, @Category, @ColorID, @Season, @ImageURL, @DateAdded, @WashAfterUses, NULL)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", item.UserID); // int
                command.Parameters.AddWithValue("@Category", item.Category); // string
                command.Parameters.AddWithValue("@ColorID", item.ColorID); // int
                command.Parameters.AddWithValue("@Season", item.Season); // string
                command.Parameters.AddWithValue("@ImageURL", item.ImageURL); // string
                command.Parameters.AddWithValue("@DateAdded", item.DateAdded ?? DateTime.Now); // DateTime או null
                command.Parameters.AddWithValue("@WashAfterUses", item.WashAfterUses); // int

                await command.ExecuteNonQueryAsync();
            }
        }
    }

}
