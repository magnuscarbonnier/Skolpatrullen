using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Du måste skriva ditt förnamn")]
        [StringLength(50, ErrorMessage = "Ditt förnamn måste vara minst 2 tecken långt", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste skriva minst ett efternamn")]
        [StringLength(50, ErrorMessage = "Ditt efternamn måste vara minst 2 tecken långt", MinimumLength = 2)]
        public string LastNames { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Du måste skriva ditt personnummer")]
        [StringLength(13, ErrorMessage = "Skriv hela ditt personnummer, 12 siffror, 13 tecken med bindestreck")]
        [RegularExpression(@"^\d{8}-\d{4}", ErrorMessage = "Skriv hela ditt personnummer, 12 siffror, 13 tecken med bindestreck")]
        public string SocialSecurityNr { get; set; }
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "Du måste skapa ett lösenord")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara mellan 8 och 30 tecken långt")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Ditt lösenord måste innehålla minst 1 siffra, 1 liten bokstav och en stor bokstav")]
        public string Password { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara mellan 8 och 30 tecken långt")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Ditt lösenord måste innehålla minst 1 siffra, 1 liten bokstav och en stor bokstav")]
        public string RePassword { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }

        public User ToUser()
        {
            return new User
            {
                FirstName = this.FirstName,
                LastNames = this.LastNames,
                Phone = this.Phone,
                SocialSecurityNr = this.SocialSecurityNr,
                Email = this.Email,
                Password = this.Password,
                Address = this.Address,
                City = this.City,
                PostalCode = this.PostalCode
            };
        }
    }
}
