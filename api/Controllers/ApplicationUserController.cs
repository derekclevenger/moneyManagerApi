using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.User.Count() == 0)
            {
                _context.User.Add(new ApplicationUser { FirstName = "derek", LastName = "clevenger", Email = "pdc2189@icloud.com", Password = "bObsBaby!2" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.User.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(int id)
        {
            var user = _context.User.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ApplicationUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            user.Salt = Salted();
            user.Password = HashPassword(user.Password, user.Salt);

            _context.User.Add(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ApplicationUser UserToUpdate)
        {
            if (UserToUpdate == null && UserToUpdate.Id != id)
            {
                return BadRequest();
            }

            var user = _context.User.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(UserToUpdate.Password))
            {
                user.Salt = Salted();
                user.Password = HashPassword(UserToUpdate.Password, user.Salt);
            }
            user.FirstName = (string.IsNullOrEmpty(UserToUpdate.FirstName)) ? user.FirstName : UserToUpdate.FirstName;
            user.LastName = (string.IsNullOrEmpty(UserToUpdate.LastName)) ? user.LastName : UserToUpdate.LastName;
            user.Email = (string.IsNullOrEmpty(UserToUpdate.Email)) ? user.Email : UserToUpdate.Email;


            _context.User.Update(user);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.User.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();
            return new NoContentResult();
        }
        private byte[] Salted()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string HashPassword(string password, byte[] salt)
        {
             // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


            return hashed;

        }
    }
}
