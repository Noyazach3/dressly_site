using dressly_site.Data;
using dressly_site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClothingItemsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ClothingItemsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // מחזיר את כל פריטי הלבוש
    [HttpGet]
    public async Task<IActionResult> GetAllClothingItems()
    {
        var clothingItems = await _dbContext.ClothingItems.ToListAsync();
        return Ok(clothingItems);
    }

    // מוסיף פריט חדש
    [HttpPost]
    public async Task<IActionResult> AddClothingItem(ClothingItem newItem)
    {
        _dbContext.ClothingItems.Add(newItem);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllClothingItems), new { id = newItem.ItemID }, newItem);
    }

    //עדכון פריט 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClothingItem(int id, ClothingItem updatedItem)
    {
        var existingItem = await _dbContext.ClothingItems.FindAsync(id);

        if (existingItem == null)
        {
            return NotFound();
        }

        existingItem.Category = updatedItem.Category;
        existingItem.ColorID = updatedItem.ColorID;
        existingItem.Season = updatedItem.Season;
        existingItem.ImageUrl = updatedItem.ImageUrl;
        existingItem.DateAdded = updatedItem.DateAdded;
        existingItem.LastWornDate = updatedItem.LastWornDate;
        existingItem.WashAfterUses = updatedItem.WashAfterUses;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }


    //מחיקת פריט
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClothingItem(int id)
    {
        var item = await _dbContext.ClothingItems.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        _dbContext.ClothingItems.Remove(item);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

}
