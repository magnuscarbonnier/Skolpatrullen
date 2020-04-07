using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class StartBlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
