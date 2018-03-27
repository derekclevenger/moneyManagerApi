using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Cors;

namespace api.Controllers
{
    [AllowAnonymous]
   
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IConfiguration _config;


        public TokenController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]Login login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddYears(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private ApplicationUser Authenticate(Login login)
        {
            ApplicationUser user = null;
             user = _context.User.FirstOrDefault(x => x.Email == login.Email);
            if (user == null)
            {
                return user;
            }

            if (user.Email == login.Email)
            {
                var loginPassword = HashPassword(login.Password, user.Salt);
                if ( loginPassword == user.Password)
                {
                    return user;

                }
            }
            return null;
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
