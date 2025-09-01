using InventorySales.DTOs.Product;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly iProduct _pro;
        private readonly iAuditLog _auditLog;
        public ProductController(iProduct pro, iAuditLog auditLog)
        {
            _pro = pro;
            _auditLog = auditLog;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _pro.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _pro.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> Create([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product
            {
                Name = request.Product.Name,
                Description = request.Product.Description,
                Price = request.Product.Price,
                Stock = request.Product.Stock,
                CategoryId = request.Product.CategoryId,
                CreatedAt = DateTime.Now
            };

            await _pro.Add(product);

            await _auditLog.LogAction(
                userId: request.UserId,
                action: "Create",
                tableName: "Product",
                recordId: product.ProductId
            );

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Update(int id, [FromBody] ProductDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _pro.GetProductById(id);
            if (existing == null) return NotFound();

            // Mapear manualmente del DTO al modelo existente
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.CategoryId = dto.CategoryId;

            await _pro.Update(existing);

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _pro.GetProductById(id);
            if (existing == null) return NotFound();

            await _pro.Delete(existing);
            return NoContent();
        }
    }
}
