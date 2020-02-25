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
    public class AsId
    {
        public int Id { get; set; }
        public AsId(int id)
        {
            Id = id;
        }
    }
}
