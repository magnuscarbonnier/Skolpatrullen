using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class APIResponse<T>
    {
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool Success { get; set; }
    }
}
