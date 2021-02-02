using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        
        public string FullName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [RegularExpression("^0\\d{10}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Enter your Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [NotMapped]
        [Compare("Email", ErrorMessage ="Email and Confirm Email don't match")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Enter your Password")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
      
        public string Password { get; set; }

        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password don't match")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Book")]
        public int BookID { get; set; }

        public Book book { get; set; }

        [DisplayName("Image")]
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
