using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ClassLibrary1.Models;

public class ClothingService
{
    private readonly HttpClient _httpClient;

    public ClothingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClothingItem>> GetClothingItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ClothingItem>>("api/Clothing");
    }

    public async Task AddClothingItemAsync(ClothingItem item)
    {
        await _httpClient.PostAsJsonAsync("api/Clothing/add", item);
    }
}
