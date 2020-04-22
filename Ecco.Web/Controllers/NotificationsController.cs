using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecco.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<EccoUser> _userManager;
        private NotificationService _notifications;

        [HttpPost("RegisterDevice")]
        public async Task<IActionResult> RegisterDevice([FromBody]DeviceRegistration deviceRegistration)
        {
            if (deviceRegistration != null)
            {
                await _notifications.RegisterDevice(deviceRegistration, _context, _userManager);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}