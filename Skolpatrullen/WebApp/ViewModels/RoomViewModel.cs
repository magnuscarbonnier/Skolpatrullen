using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RoomViewModel
    {
        [Required]
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public IEnumerable<School> SchoolList { get; set; }

        public Room ToRoom()
        {
            return new Room
            {
                Name = this.Name,
                SchoolId = this.SchoolId
            };
        }
    }
}
