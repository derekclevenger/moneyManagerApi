using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        [HttpGet("{userId}", Name = "GetTransactions")]
        public IActionResult GetById(int userId)
        {
            var transactions = _context.Transactions.Where(x => x.UserId == userId).ToList();
            if (transactions == null)
            {
                return NotFound();
            }
            return new ObjectResult(transactions);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return new ObjectResult(transaction);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transaction TransactionToUpdate)
        {
            if (TransactionToUpdate == null && TransactionToUpdate.Id != id)
            {
                return BadRequest();
            }

            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = TransactionToUpdate.Amount != 0 ? TransactionToUpdate.Amount : transaction.Amount;
            transaction.Payee = (string.IsNullOrEmpty(TransactionToUpdate.Payee)) ? transaction.Payee : TransactionToUpdate.Payee;
            transaction.UserId = TransactionToUpdate.UserId;
            transaction.Category = (string.IsNullOrEmpty(TransactionToUpdate.Category)) ? transaction.Category : TransactionToUpdate.Category;
            transaction.TransactionDate = new DateTime();

            _context.Transactions.Update(transaction);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
