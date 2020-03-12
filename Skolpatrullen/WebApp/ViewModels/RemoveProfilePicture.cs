using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RemoveProfilePicture
    {
        public int UserId { get; set; }
        public byte[] Picture { get; set; }
    }
}
