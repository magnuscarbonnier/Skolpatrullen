using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AdminViewModel
    {
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public IEnumerable<School> SchoolList { get; set; }
        public IEnumerable<User> UserList { get; set; }

        public UserSchool ToUserSchool()
        {
            return new UserSchool
            {
                 UserId = this.UserId,
                 SchoolId = this.SchoolId
            };
        }
    }
}
