using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : Controller
    {
        private readonly iAuditLog _adt;
        public AuditLogController(iAuditLog adt)
        {
            _adt = adt;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuditLog>))]
        public async Task<ActionResult<List<AuditLog>>> GetAll()
        {
            var logs = await _adt.GetAllAuditLogs();
            return Ok(logs);
        }
    }
}
