using System;
using System.ComponentModel.DataAnnotations;

namespace api
{
    public class Categories
    {
     
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Category")]
        public string Category { get; set; }
    }
}
