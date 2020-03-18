using System;
using System.Collections.Generic;

namespace Lib
{
    public static class Constants
    {
        public const string AcceptCP = "Godkänn ansökan";
        public const string DenyCP = "Avslå ansökan";
        public const string AcceptAsTeacherCP = "Registrera som lärare";
    }
    public static class Grade
    {
        public const string Ej_Betygsatt = null;
        public const string IG = "IG";
        public const string G = "G";
        public const string VG = "VG";
        public const string MVG = "MVG";
    }
    public class TokenBody
    {
        public string token { get; set; }
    }
    public class APIResponse
    {
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string FailureMessage { get; set; }
    }
    public class APIResponse<T> : APIResponse
    {
        public T Data { get; set; }
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
