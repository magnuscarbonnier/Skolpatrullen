using System;
using System.Collections.Generic;

namespace Lib
{
    public class TokenBody
    {
        public string token { get; set; }
    }
    public class APIResponse<T>
    {
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool Success { get; set; }
    }
    public class ChangePasswordBody
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangeProfilePictureBody
    {
        public int UserId { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileExtension { get; set; }
    }
    public class AsId
    {
        public int Id { get; set; }
        public AsId(int id)
        {
            Id = id;
        }
    }
}
