using InventorySales.DTOs.Customer;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly iCustomer _cus;
        private readonly iAuditLog _auditLog;
        public CustomerController(iCustomer customer, iAuditLog auditLog)
        {
            _cus = customer;
            _auditLog = auditLog;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Customer>))]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            var customers = await _cus.GetAllCustomers();
            return Ok(customers);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _cus.GetCustomerById(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetByName(string name)
        {
            var customer = await _cus.GetCustomerByName(name);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetByEmail(string email)
        {
            var customer = await _cus.GetCustomerByEmail(email);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Customer>))]
        public async Task<ActionResult<List<Customer>>> SearchCustomers( [FromQuery] string term, [FromQuery] int limit = 10)
        {
            var results = await _cus.SearchCustomer(term, limit);
            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customer = new Customer
            {
                FirstName = request.Customer.FirstName,
                LastName = request.Customer.LastName,
                Email = request.Customer.Email,
                Phone = request.Customer.Phone,
                CreatedAt = DateTime.Now,
            };

            await _cus.Add(customer);

            await _auditLog.LogAction(
                userId: request.UserId,
                action: "Created",
                tableName: "Customer",
                recordId: customer.CustomerId
                );

            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDTO>> Update(int id, [FromBody] CustomerDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _cus.GetCustomerById(id);
            if (existing == null) return NotFound();

            // Mapear manualmente del DTO al modelo existente
            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;

            await _cus.Update(existing);

            return Ok(existing);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _cus.GetCustomerById(id);
            if (existing == null) return NotFound();

            await _cus.Delete(existing);
            return NoContent();
        }
    }
}
