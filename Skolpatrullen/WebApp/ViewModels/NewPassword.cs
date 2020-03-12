using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class NewPassword
    {
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage = "Du måste skapa ett lösenord")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara mellan 8 och 30 tecken långt")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Ditt lösenord måste innehålla minst 1 siffra, 1 liten bokstav och en stor bokstav")]
        public string Password { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara mellan 8 och 30 tecken långt")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Ditt lösenord måste innehålla minst 1 siffra, 1 liten bokstav och en stor bokstav")]
        public string RePassword { get; set; }
    }
}
