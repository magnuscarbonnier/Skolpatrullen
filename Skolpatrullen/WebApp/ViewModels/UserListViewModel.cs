using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<User> UserList { get; set; }
        public string Search { get; set; }
    }
   
}
