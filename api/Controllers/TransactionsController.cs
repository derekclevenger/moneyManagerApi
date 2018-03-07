using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
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
            return _context.Transaction.ToList();
        }

        [HttpGet("{userId}", Name = "GetTransactions")]
        public IActionResult GetById(int userId)
        {
            var transactions = _context.Transaction.Where(x => x.UserId == userId).ToList();
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

            _context.Transaction.Add(transaction);
            _context.SaveChanges();

            return CreatedAtRoute("GetCategory", new { id = transaction.Id }, transaction);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transaction TransactionToUpdate)
        {
            if (TransactionToUpdate == null && TransactionToUpdate.Id != id)
            {
                return BadRequest();
            }

            var transaction = _context.Transaction.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = TransactionToUpdate.Amount != 0 ? TransactionToUpdate.Amount : transaction.Amount;
            transaction.Payee = (string.IsNullOrEmpty(TransactionToUpdate.Payee)) ? transaction.Payee : TransactionToUpdate.Payee;
            transaction.UserId = TransactionToUpdate.UserId;
            transaction.Category = (string.IsNullOrEmpty(TransactionToUpdate.Category)) ? transaction.Category : TransactionToUpdate.Category;
            transaction.TransactionDate = new DateTime();

            _context.Transaction.Update(transaction);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var transaction = _context.Transaction.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
