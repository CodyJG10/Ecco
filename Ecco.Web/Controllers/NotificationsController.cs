
using Ecco.Entities;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Ecco.Web.Models;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecco.Web.Controllers
{
    [Route("notifications")]
    public class NotificationsController : Controller
    {
        private readonly NotificationService _notifications;
        private readonly UserManager<EccoUser> _userManager;
        private readonly EventsHubService _eventsHubService;

        public NotificationsController(NotificationService notifications, 
            UserManager<EccoUser> userManager,
            EventsHubService eventsHubService)
        {
            _notifications = notifications;
            _userManager = userManager;
            _eventsHubService = eventsHubService;
        }

        [HttpGet("Register")]
        public async Task<IActionResult> CreatePushRegistrationId()
        {
            var registrationId = await _notifications.CreateRegistrationId();
            return Ok(registrationId);
        }

        [HttpPut("Enable/{id}")]
        public async Task<IActionResult> RegisterForPushNotifications([FromRoute]string id, [FromBody] DeviceRegistration deviceUpdate)
        {
            var registrationResult = await _notifications.RegisterForPushNotifications(id, deviceUpdate, _userManager);

            if (registrationResult == true)
                return Ok();
            else
                return BadRequest();
        }

        [Route("Send")]
        [Route("Index")]
        [Route("")]
        [Authorize("Admin")]
        public IActionResult Send()
        {
            SendNotificationModel model = new SendNotificationModel()
            {
                Message = "Hello, World!"
            };

            return View("Send", model);
        }

        [HttpPost("SendNotification")]
        [Authorize("Admin")]
        public async Task<IActionResult> SendNotification([FromForm]SendNotificationModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            _notifications.SendNotification(model.Message, user); 
            return RedirectToAction("Send");
        }

        [HttpGet("TestEvents")]
        public IActionResult TestEvents(string content)
        {
            _eventsHubService.SendEvent(null);
            return Ok("Succesfully sent");
        }
    }
}