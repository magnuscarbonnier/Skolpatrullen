using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AdminChangeProfileViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Du måste skriva ditt förnamn")]
        [StringLength(50, ErrorMessage = "Förnamn måste vara minst 2 tecken långt", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste skriva minst ett efternamn")]
        [StringLength(50, ErrorMessage = "Efternamn måste vara minst 2 tecken långt", MinimumLength = 2)]
        public string LastNames { get; set; }
        [Required(ErrorMessage = "Du måste skriva ditt personnummer")]
        [StringLength(13, ErrorMessage = "Skriv hela personnumret, 12 siffror, 13 tecken med bindestreck")]
        [RegularExpression(@"^\d{8}-\d{4}", ErrorMessage = "Skriv hela personnumret, 12 siffror, 13 tecken med bindestreck")]
        public string SocialSecurityNr { get; set; }
        public bool IsSuperUser { get; set; }
        public byte[] ProfilePicture { get; set; }
        public User User { get; set; }
    }
}
