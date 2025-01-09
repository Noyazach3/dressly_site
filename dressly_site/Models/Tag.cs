using System.ComponentModel.DataAnnotations;

namespace dressly_site.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        public string TagName { get; set; }

        public ICollection<ClothingItemTag> ClothingItemTags { get; set; }
    }
}
