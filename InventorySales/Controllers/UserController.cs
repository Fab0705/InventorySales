using InventorySales.DTOs.User;
using InventorySales.Models;
using InventorySales.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventorySales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly iUser _usr;
        private readonly iAuditLog _auditLog;
        public UserController(iUser usr, iAuditLog auditLog)
        {
            _usr = usr;
            _auditLog = auditLog;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _usr.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _usr.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpGet("by-name/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByUsrName(string username)
        {
            var user = await _usr.GetUserByName(username);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpGet("by-role/{role}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByRole(string role)
        {
            var users = await _usr.GetAllUsersByRole(role);
            return Ok(users);
        }
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Create([FromBody] UserAuthenticateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _usr.Authenticate(dto.Username, dto.Password);

            return Ok(user);
        }
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Create([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                Username = request.User.Username,
                PasswordHash = request.User.PasswordHash,
                Role = request.User.Role,
                CreatedAt = DateTime.Now
            };

            await _usr.Add(user);

            await _auditLog.LogAction(
                userId: request.UserId,
                action: "Create",
                tableName: "User",
                recordId: user.UserId
                );

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _usr.GetUserById(id);
            if (existing == null) return NotFound();

            existing.Username = dto.Username;
            existing.PasswordHash = dto.PasswordHash;
            existing.Role = dto.Role;

            await _usr.Update(existing);

            return Ok(existing);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _usr.GetUserById(id);
            if (existing == null) return NotFound();

            await _usr.Delete(existing);
            return NoContent();
        }
    }
}
