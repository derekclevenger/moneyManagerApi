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
    //TODO implement CRUD for BUDGET
    [EnableCors("AllowAllOrigins")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class BudgetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("GetAll")]
        public IEnumerable<Budget> GetAll()
        {
            return _context.Budget.ToList();
        }

        [HttpGet("{userId}", Name = "GetByUser")]
        [ActionName("GetByUser")]
        public IActionResult GetByUserId(int userId)
        {
            var budgets = _context.Budget.Where(x => x.UserId == userId).ToList();
            if (budgets == null)
            {
                return NotFound();
            }
            return new ObjectResult(budgets);
        }

        [HttpGet("{Id}", Name = "GetById")]
        [ActionName("GetById")]
        public IActionResult GetById(int Id)
        {
            var budget = _context.Budget.Where(x => x.Id == Id);
            if (budget == null)
            {
                return NotFound();
            }
            return new ObjectResult(budget);
        }

        [HttpPost]
        public IActionResult AddBudget([FromBody] Budget budget)
        {
            if (budget == null)
            {
                return BadRequest();
            }

            _context.Budget.Add(budget);
            _context.SaveChanges();

            return new ObjectResult(budget);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBudget(int id, [FromBody] Budget BudgetToUpdate)
        {
            if (BudgetToUpdate == null && BudgetToUpdate.Id != id)
            {
                return BadRequest();
            }

            var budget = _context.Budget.FirstOrDefault(t => t.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            budget.Amount = BudgetToUpdate.Amount != 0 ? BudgetToUpdate.Amount : budget.Amount;
            budget.UserId = BudgetToUpdate.UserId;
            budget.Category = (string.IsNullOrEmpty(BudgetToUpdate.Category)) ? budget.Category : BudgetToUpdate.Category;

            _context.Budget.Update(budget);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(int id)
        {
            var budget = _context.Budget.FirstOrDefault(t => t.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budget.Remove(budget);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
