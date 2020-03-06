using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApp.ViewModels
{
    public class ProfileCombinedViewModel
    {
        public ProfileViewModel PVM { get; set; } = new ProfileViewModel();
        public ChangePasswordViewModel CPVM { get; set; } = new ChangePasswordViewModel();
        public ChangeProfilePictureViewModel CPPVM { get; set; } = new ChangeProfilePictureViewModel();
    }
    public class ProfileViewModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public IEnumerable<CourseParticipant> CourseParticipantList { get; set; }
        public IEnumerable<Course> CourseList { get; set; }
        public User User { get; set; }
        public User ToUser()
        {
            return new User
            {
                Phone = this.Phone,
                Email = this.Email,
                Address = this.Address,
                City = this.City,
                PostalCode = this.PostalCode,
                
            };
        }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Fyll i fältet")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Ditt lösenord måste innehålla minst 1 siffra, 1 liten bokstav och en stor bokstav")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara mellan 8 och 30 tecken långt")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        public string ReNewPassword { get; set; }
    }

    public class ChangeProfilePictureViewModel
    {
        public IFormFile file { get; set; }

    }
}
