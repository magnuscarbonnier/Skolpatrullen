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
        public string FirstName { get; set; }
        [Required]
        public string LastNames { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string SocialSecurityNr { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(8, 16)]
        public string Password { get; set; }
        [Required]
        [Range(8, 16)]
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
