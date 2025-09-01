using System.ComponentModel.DataAnnotations;

namespace InventorySales.DTOs.Sale
{
    public class SaleRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public SaleCreateDTO Sale {  get; set; }
    }
}
