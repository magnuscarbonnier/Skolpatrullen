using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<RoomController> _logger;
        public RoomController(Context context, ILogger<RoomController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public ActionResult<Room> Add(Room room)
         {
            if (room != null)
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
            }
            return CreatedAtAction(nameof(Add), new { id = room.Id }, room);
        }
    }
}