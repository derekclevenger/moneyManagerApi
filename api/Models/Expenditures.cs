using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Expenditures
    {
        [Display(Name = "Category")]
        public string Category { get; set; }
     
        [Required(ErrorMessage = "Required")]
        [Display(Name = "BudgetedAmount")]
        public decimal BudgetedAmount { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Amount")]
        public decimal SpentAmount { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Amount")]
        public decimal TotalAmount { get; set; }


    }
}
