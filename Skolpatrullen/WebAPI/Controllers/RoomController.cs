using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class RoomController : APIController
    {
        public RoomController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<Room> Add(Room room)
         {
            APIResponse<Room> response = new APIResponse<Room>();
            if (room != null)
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
                response.Data = room;
                response.Success = true;
                response.SuccessMessage = $"La till rum med namn {room.Name}";
            }
            return response;
        }
    }
}