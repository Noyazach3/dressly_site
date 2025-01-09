using Microsoft.EntityFrameworkCore;
using dressly_site.Models; // ודא שכוללת את המודל שלך

namespace dressly_site.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ClothingItem> ClothingItems { get; set; }
        public DbSet<Outfit> Outfits { get; set; }
        public DbSet<OutfitItem> OutfitItems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ClothingItemTag> ClothingItemTags { get; set; }
        public DbSet<ClothingItemUsage> ClothingItemUsages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Color> Colors { get; set; }
    }
}
