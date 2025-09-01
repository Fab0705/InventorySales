using System.ComponentModel.DataAnnotations;

namespace InventorySales.DTOs.User
{
    public class UserRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public UserDTO User { get; set; }
    }
}
