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
    [Route("api/[controller]/[action]")]
    public class TransactionsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{Id}", Name = "GetAll")]
        [ActionName("GetAll")]
        public IActionResult GetAll(int Id)
        {
            DateTime date = DateTime.Now;
     
            List<Transaction> newTransactions = new List<Transaction>();



            var firstOfYear = new DateTime(date.Year,1, 1);
            var transactions = _context.Transactions.Where(x => x.UserId == Id && x.TransactionDate > firstOfYear).OrderBy(z => z.Category).ToList();
            var categoryList = transactions.Select(x => x.Category).Distinct();


            foreach (var item in categoryList)
            {
                newTransactions.Add(new Transaction
                {
                    Id = 0,
                    Amount = transactions.FindAll(y => y.Category == item).Sum(x => x.Amount),
                    Category = item,
                    Payee = item,
                    TransactionDate = new DateTime(),
                    AccountType = "checking",
                    UserId = Id
                });
            }

            var budgets = _context.Budget.Where(x => x.UserId == Id).OrderBy(z => z.Category);

            var expenditures = new List<Expenditures>();

            //foreach(var budget in budgets){
            //    foreach(var transaction in newTransactions){
            //        if(budget.Category.ToLower() == transaction.Category.ToLower()) {
            //            expenditures.Add(new Expenditures
            //            {
            //                Category = budget.Category,
            //                BudgetedAmount = budget.Monthly ? budget.Amount * 12 : budget.Amount,
            //                SpentAmount = transaction.Amount,
            //                TotalAmount = budget.Monthly ? (budget.Amount * 12) + transaction.Amount : budget.Amount + transaction.Amount,
            //            });
            //        }
            //    }
            //}


     
            foreach (var budget in budgets)
            {
                var counter = 0;
                foreach (var transaction in newTransactions)
                {

                    if (budget.Category.ToLower() == transaction.Category.ToLower())
                    {
                        counter++;
                        expenditures.Add(new Expenditures
                        {
                            Category = budget.Category,
                            BudgetedAmount = budget.Monthly ? budget.Amount * 12 : budget.Amount,
                            SpentAmount = transaction.Amount,
                            TotalAmount = budget.Monthly ? (budget.Amount * 12) + transaction.Amount : budget.Amount + transaction.Amount,
                        });                    }
                }
                if (counter == 0)
                {
                    expenditures.Add(new Expenditures
                    {
                        Category = budget.Category,
                        BudgetedAmount = budget.Monthly ? budget.Amount * 12 : budget.Amount,
                        SpentAmount = 0,
                        TotalAmount = budget.Monthly ? budget.Amount * 12 : budget.Amount,
                    });
                }
            }
         
           //var expenditure = expenditures.GroupBy(x => x.Category).Distinct();
           return new ObjectResult(expenditures);
        }

        [HttpGet("{userId}", Name = "GetByUserID")]
        [ActionName("GetByUserID")]
        public IActionResult GetByUserID(int userId)
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
            transaction.TransactionDate = TransactionToUpdate.TransactionDate;
            transaction.AccountType = TransactionToUpdate.AccountType;

            _context.Transactions.Update(transaction);
            _context.SaveChanges();
            return new ObjectResult(transaction);
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
