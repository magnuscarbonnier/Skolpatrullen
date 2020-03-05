using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class APIController : ControllerBase
    {
        public readonly Context _context;
        public readonly ILogger<UserController> _logger;
        public APIController(Context context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
