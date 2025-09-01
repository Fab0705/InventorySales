using System.ComponentModel.DataAnnotations;

namespace InventorySales.DTOs.Category
{
    public class CategoryRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public CategoryDTO Category { get; set; }
    }
}
