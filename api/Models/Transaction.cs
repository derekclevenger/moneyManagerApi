using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Payee")]
        public string Payee { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Category")]        
         public string Category { get; set; }

    }
}
