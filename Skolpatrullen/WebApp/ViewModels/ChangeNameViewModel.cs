using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class ChangeNameViewModel
    {
       
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public User User { get; set; }

        public User ToUser()
        {
            return new User
            {
                FirstName = this.FirstName,
                LastNames = this.LastNames,
            };
        }
    }
}
