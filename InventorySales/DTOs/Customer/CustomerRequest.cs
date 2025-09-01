using System.ComponentModel.DataAnnotations;

namespace InventorySales.DTOs.Customer
{
    public class CustomerRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public CustomerDTO Customer { get; set; }
    }
}
