using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class AssignmentFile
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int AssignmentId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public AssignmentFileType Type { get; set; }
        public Assignment Assignment{ get; set; }
        public File File { get; set; }
    }
    public enum AssignmentFileType
    {
        StudentFile = 0,
        AssignmentFile = 1
    }
}
