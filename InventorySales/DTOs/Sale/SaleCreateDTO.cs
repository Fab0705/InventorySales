namespace InventorySales.DTOs.Sale
{
    public class SaleCreateDTO
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public List<SaleDetailCreateDTO> SaleDetails { get; set; }
    }
}
