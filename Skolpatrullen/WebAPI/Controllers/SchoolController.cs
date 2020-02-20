using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<SchoolController> _logger;
        public SchoolController(Context context, ILogger<SchoolController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/GetAllSchools")]
        public APIResponse<IEnumerable<School>> GetAllSchools()
        {
            APIResponse<IEnumerable<School>> response = new APIResponse<IEnumerable<School>>();
            var schoollist = _context.Schools.OrderBy(s => s.Name).ToList();
            if (schoollist != null)
            {
                response.Success = true;
                response.Data = schoollist;
            }
            return response;
        }
    }
}