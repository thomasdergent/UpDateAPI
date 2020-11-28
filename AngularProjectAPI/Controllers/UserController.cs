using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using AngularProjectAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUcommentService _userService;
        private readonly NewsContext _context;

        public UserController(IUcommentService userService, NewsContext context)
        {
            _userService = userService;
            _context = context;
        }

            [Authorize]
            [HttpGet]
            public async Task<ActionResult<IEnumerable<User>>> GetUsers()
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
                return await _context.Users.ToListAsync();
            }

        [HttpGet("role/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByRole(int id)
        {
            var user = await _context.Users
                                         .Where(r => r.RoleID == id).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Gebruiker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("userName/{userName}")]
        public async Task<ActionResult<User>> GetUserByUserName(string userName)
        {
            var user = await _context.Users
                                         .Where(u => u.UserName == userName).FirstAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _context.Users
                                         .Where(u => u.Email == email).FirstAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var newUser = await _userService.PostUser(user);
            return Ok(newUser);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}