namespace InventorySales.DTOs.Sale
{
    public class SaleDetailDTO
    {
        public int SaleDetailId { get; set; }
        public int? ProductId { get; set; }

        public ProductSummaryDTO Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
