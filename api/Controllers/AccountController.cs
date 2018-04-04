using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;

            //if (_context.Account.Count() == 0) 
            //{ 
            //    _context.Account.Add(new Account {Amount = 100.00M , Type = "check", UserId = 26}); 
            //    _context.SaveChanges(); 
            //} 
        }


        [HttpGet]
        public IEnumerable<Account> GetAll()
        {
            return _context.Account.ToList();
        }

        [HttpGet("{userId}", Name = "GetAccounts")]
        public IActionResult GetById(int userId)
        {
            var accounts = _context.Account.Where(x => x.UserId == userId).ToList();
            if (accounts == null)
            {
                return NotFound();
            }
            return new ObjectResult(accounts);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest();
            }
            account.Amount = (decimal)account.Amount;
            _context.Account.Add(account);
            _context.SaveChanges();

            //return CreatedAtRoute("GetAccounts", new { id = account.Id }, account);
            return new ObjectResult(account);

        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Account AccountToUpdate)
        {
            if (AccountToUpdate == null && AccountToUpdate.Id != id)
            {
                return BadRequest();
            }

            var account = _context.Account.FirstOrDefault(t => t.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            account.Amount = AccountToUpdate.Amount != 0 ? AccountToUpdate.Amount : account.Amount;
            account.Type = (string.IsNullOrEmpty(AccountToUpdate.Type)) ? account.Type : AccountToUpdate.Type;
            account.UserId = AccountToUpdate.UserId;

            _context.Account.Update(account);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _context.Account.FirstOrDefault(t => t.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            _context.SaveChanges();
            return new NoContentResult();
        }


    }
}
