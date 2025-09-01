using InventorySales.DTOs;
using InventorySales.DTOs.Sale;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.EntityFrameworkCore;

namespace InventorySales.Repository
{
    public class SaleRepository : iSale
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(Sale sale, List<SaleDetail> saleDetail)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                dbContext.Add(sale);
                await dbContext.SaveChangesAsync();
                foreach (var detail in saleDetail)
                {
                    var product = await dbContext.Products.FindAsync(detail.ProductId);

                    if (product == null)
                        throw new Exception($"Product with ID {detail.ProductId} not found.");

                    if (product.Stock < detail.Quantity)
                        throw new Exception($"Insufficient stock for the product '{product.Name}'. Cuerrent stock: {product.Stock}");

                    product.Stock -= detail.Quantity;

                    detail.SaleId = sale.SaleId;
                    dbContext.SaleDetails.Add(detail);
                }
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error registering sale: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Sale>> GetAllSale() => await dbContext.Sales.ToListAsync();

        public async Task<SalesResponseDTO?> GetDetailedSaleById(int id)
        {
            var sale = await dbContext.Sales
                .Include(s => s.Customer)
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.SaleId == id);

            if (sale == null) return null;

            return new SalesResponseDTO
            {
                SaleId = sale.SaleId,
                UserId = sale.UserId,
                Username = sale.User.Username,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer.FirstName + " " + sale.Customer.LastName,
                Email = sale.Customer.Email,
                SaleDetails = sale.SaleDetails.Select(ds => new SaleDetailDTO
                {
                    SaleDetailId = ds.SaleDetailId,
                    ProductId = ds.ProductId,
                    Product = new ProductSummaryDTO
                    {
                        ProductId = ds.Product.ProductId,
                        Name = ds.Product.Name,
                        Price = ds.Product.Price
                    },
                    Quantity = ds.Quantity,
                    UnitPrice = ds.UnitPrice,
                    Subtotal = ds.Subtotal
                }).ToList()
            };
        }
    }
}
