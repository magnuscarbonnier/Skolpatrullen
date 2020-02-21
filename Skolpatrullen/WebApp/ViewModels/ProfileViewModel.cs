using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class ProfileViewModel : LayoutViewModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Password { get; set; }

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
}
