using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrdersProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        public string Address { get; set; }
        [StringLength(12, ErrorMessage = "Mobile number between 10 to 12 digits", MinimumLength = 10)]
        public string MobileNo { get; set; }
    }
}