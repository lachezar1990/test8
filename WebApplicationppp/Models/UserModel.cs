using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationppp.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Електронната поща е задължителна!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Електронната поща не е валидна")]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна!")]
        [StringLength(100, ErrorMessage = "{0}та трябва да е най-малко {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторете паролата")]
        [Compare("Password", ErrorMessage = "Двете пароли не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}