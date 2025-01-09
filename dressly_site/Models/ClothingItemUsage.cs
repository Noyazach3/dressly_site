using System.ComponentModel.DataAnnotations;

namespace dressly_site.Models
{
    public class ClothingItemUsage
    {
        [Key]
        public int UsageID { get; set; }
        public int ItemID { get; set; }
        public int WearCount { get; set; }

        public ClothingItem ClothingItem { get; set; }
    }
}
