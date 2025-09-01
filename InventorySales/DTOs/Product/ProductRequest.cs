using System.ComponentModel.DataAnnotations;

namespace InventorySales.DTOs.Product
{
    public class ProductRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public ProductDTO Product { get; set; }
    }
}
