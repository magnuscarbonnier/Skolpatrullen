using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SchoolViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public School ToSchool()
        {
            return new School
            {
                Name = this.Name,
                Address = this.Address,
                Phone = this.Phone,
                City = this.City,
                PostalCode = this.PostalCode
            };
        }

    }
}
