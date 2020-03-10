using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class PasswordRecovery
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpireTime { get; set; }
        public User User { get; set; }
    }
}
