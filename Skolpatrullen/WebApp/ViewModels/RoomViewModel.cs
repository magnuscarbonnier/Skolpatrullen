using Database.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RoomViewModel
    {
        [Required]
        public string Name { get; set; }
        public string SchoolId { get; set; }
        
        public Room ToRoom()
        {
            return new Room
            {
                Name = this.Name,
                SchoolId = Int32.Parse(this.SchoolId)
            };
        }
    }
}
