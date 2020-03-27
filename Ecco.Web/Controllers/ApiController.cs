using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecco.Web.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public ApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("Cards")]
        public List<Card> Cards()
        {
            return _context.Cards.ToList();
        }

        [HttpGet("Connections")]
        public List<Connection> Connections(Guid id)
        {
            return _context.Connections.Where(x => x.FromId == id || x.ToId == id).ToList();
        }

        [HttpGet("UserInfo")]
        public async Task<IdentityUser> GetUserInfo()
        {
            var allClaims = User.Claims.ToList();
            var name = allClaims.First(c => c.Type.Contains("nameidentifier")).Value;
            return await _userManager.FindByNameAsync(name);
        }
    }
}