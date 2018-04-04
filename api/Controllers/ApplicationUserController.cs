using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace api.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Salt _salt;

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [HttpPost ("{id}")]
        public IActionResult GetAuthenticatedUser(int id, [FromBody] Login login)
        {
            ApplicationUser user = null;
            user = _context.User.FirstOrDefault(x => x.Email == login.Email);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Email == login.Email)
            {
                if (_salt.HashPassword(login.Password, user.Salt) == user.Password)
                {
                    return new ObjectResult(user);

                }
            }
            return new ObjectResult(user);
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

        [EnableCors("AllowAllOrigins")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create([FromBody] ApplicationUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            user.Salt = _salt.Salted();
            user.Password = _salt.HashPassword(user.Password, user.Salt);
            user.Email = user.Email.ToLower();

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
                user.Salt = _salt.Salted();
                user.Password = _salt.HashPassword(UserToUpdate.Password, user.Salt);
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
       
    }
}
