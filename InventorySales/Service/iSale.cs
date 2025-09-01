using InventorySales.DTOs.Sale;
using InventorySales.Models;

namespace InventorySales.Service
{
    public interface iSale
    {
        Task<IEnumerable<Sale>> GetAllSale();
        Task<SalesResponseDTO> GetDetailedSaleById(int id);
        Task Add(Sale sale, List<SaleDetail> saleDetail);
    }
}
