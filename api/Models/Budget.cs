using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool Monthly { get; set; }
    }
}
