using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Account Type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Required")]
        public int UserId { get; set; }

    }
}
