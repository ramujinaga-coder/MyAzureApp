using Microsoft.AspNetCore.Mvc;

namespace MyAzureApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Role = "Admin" },
            new User { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Role = "User" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet("role/{role}")]
        public ActionResult<IEnumerable<User>> GetUsersByRole(string role)
        {
            var users = _users.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
            return Ok(users);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}