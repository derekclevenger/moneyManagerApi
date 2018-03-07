using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using Microsoft.AspNetCore.Authorization;


namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.Category.Count() == 0)
            {
                _context.Category.Add(new Categories { Category = "Groceries" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Categories> GetAll()
        {
            return _context.Category.ToList();
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult GetById(int id)
        {
            var category = _context.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return new ObjectResult(category);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Categories category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            category.Category = category.Category;

            _context.Category.Add(category);
            _context.SaveChanges();

            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Categories CategoryToUpdate)
        {
            if (CategoryToUpdate == null && CategoryToUpdate.Id != id)
            {
                return BadRequest();
            }

            var category = _context.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Category = (string.IsNullOrEmpty(CategoryToUpdate.Category)) ? category.Category : CategoryToUpdate.Category;


            _context.Category.Update(category);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
