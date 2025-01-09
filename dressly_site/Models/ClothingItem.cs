using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace dressly_site.Models
{
    public class ClothingItem
    {
        [Key] // מגדיר את ItemID כמפתח ראשי
        public int ItemID { get; set; }

        public int UserID { get; set; }
        public string Category { get; set; }
        public int ColorID { get; set; } 
        public string Season { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastWornDate { get; set; }
        public int WashAfterUses { get; set; }

        public User User { get; set; }
        public Color Color { get; set; }
        public ICollection<OutfitItem> OutfitItems { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<ClothingItemTag> ClothingItemTags { get; set; }
        public ICollection<ClothingItemUsage> ClothingItemUsages { get; set; }
    }
}
