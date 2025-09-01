namespace InventorySales.DTOs.Sale
{
    public class SalesResponseDTO
    {
        public int SaleId { get; set; }

        public int? UserId { get; set; }
        public string Username { get; set; } = null!;

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public List<SaleDetailDTO> SaleDetails { get; set; } = new();
    }
}
