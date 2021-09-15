using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInbound()
        {
            Console.WriteLine("--> Inbound call");
            return Ok(" --> Inbound Test success");
        }
    }

}