using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Controllers
{
    [Route("UserCard")]
    [ApiController]
    public class UserCardController : ControllerBase
    {
        private UserManager<EccoUser> _userManager;
        private ApplicationDbContext _context;

        public UserCardController(UserManager<EccoUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCard([FromRoute]string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string activeCard = user.ActiveCard;
            var card = _context.Cards.Single(x => x.Id == int.Parse(activeCard));
            return RedirectToAction("GetCard", "Cards", new { id = card.Id });
        }
    }
}