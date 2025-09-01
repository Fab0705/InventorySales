using InventorySales.DTOs.Category;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly iCategory _cat;
        private readonly iAuditLog _auditLog;
        public CategoryController(iCategory cat, iAuditLog auditLog)
        {
            _cat = cat;
            _auditLog = auditLog;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            var categories = await _cat.GetAllCategories();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var cateogory = await _cat.GetCategoryById(id);
            if (cateogory == null) return NotFound();
            return Ok(cateogory);
        }
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetByName(string name)
        {
            var cateogory = await _cat.GetCategoryByName(name);
            if (cateogory == null) return NotFound();
            return Ok(cateogory);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> Create([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = new Category
            {
                Name = request.Category.Name,
                Description = request.Category.Description,
            };

            await _cat.Add(category);

            await _auditLog.LogAction(
                userId: request.UserId,
                action: "Created",
                tableName: "Category",
                recordId: category.CategoryId
                );

            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Update(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _cat.GetCategoryById(id);
            if (existing == null) return NotFound();

            // Mapear manualmente del DTO al modelo existente
            existing.Name = dto.Name;
            existing.Description = dto.Description;

            await _cat.Update(existing);

            return Ok(existing);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _cat.GetCategoryById(id);
            if (existing == null) return NotFound();

            await _cat.Delete(existing);
            return NoContent();
        }
    }
}
