using InventorySales.DTOs.Sale;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : Controller
    {
        private readonly iSale _sal;
        private readonly iProduct _pro;
        private readonly iAuditLog _auditLog;
        public SaleController(iSale sal, iProduct pro, iAuditLog auditLog)
        {
            _sal = sal;
            _pro = pro;
            _auditLog = auditLog;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Sale>))]
        public async Task<ActionResult<List<Sale>>> GetAll()
        {
            var sales = await _sal.GetAllSale();
            return Ok(sales);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SalesResponseDTO>> GetById(int id)
        {
            var sale = await _sal.GetDetailedSaleById(id);
            if (sale == null) return NotFound();
            return Ok(sale);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Sale))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Sale>> Create([FromBody] SaleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Sale.SaleDetails == null || !request.Sale.SaleDetails.Any())
                return BadRequest("The sale must contain at least one product.");

            decimal total = 0;

            var sale = new Sale
            {
                CustomerId = request.Sale.CustomerId,
                UserId = request.Sale.UserId,
                SaleDate = DateTime.Now
            };

            var saleDetails = new List<SaleDetail>();
            foreach (var d in request.Sale.SaleDetails)
            {
                var product = await _pro.GetProductById(d.ProductId);
                if (product == null)
                    return BadRequest($"Product with ID {d.ProductId} doesn't exist.");

                var subtotal = d.Quantity * product.Price;
                total += subtotal;

                saleDetails.Add(new SaleDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = subtotal
                });
            }

            sale.Total = total;

            await _sal.Add(sale, saleDetails);

            await _auditLog.LogAction(
                userId: request.UserId,
                action: "Created",
                tableName: "Sale & SaleDetail",
                recordId: sale.SaleId
            );

            var response = new SalesResponseDTO
            {
                SaleId = sale.SaleId,
                UserId = sale.UserId,
                Username = sale.User?.Username ?? "",   // asumiendo que Sale tiene navegación User
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer?.FirstName + " " + sale.Customer?.LastName ?? "",
                Email = sale.Customer?.Email ?? "",
                SaleDetails = sale.SaleDetails.Select(sd => new SaleDetailDTO
                {
                    SaleDetailId = sd.SaleDetailId,
                    ProductId = sd.ProductId,
                    Product = new ProductSummaryDTO
                    {
                        ProductId = sd.Product.ProductId,
                        Name = sd.Product.Name,
                        Price = sd.Product.Price
                    },
                    Quantity = sd.Quantity,
                    UnitPrice = sd.UnitPrice,
                    Subtotal = sd.Subtotal
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = sale.SaleId }, response);
        }
    }
}
