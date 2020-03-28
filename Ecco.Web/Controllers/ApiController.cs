using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Ecco.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecco.Web.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("UserInfo")]
        public async Task<IdentityUser> GetUserInfo()
        {
            var allClaims = User.Claims.ToList();
            var name = allClaims.First(c => c.Type.Contains("nameidentifier")).Value;
            return await _userManager.FindByNameAsync(name);
        }

        #region Cards

        [HttpGet("Cards")]
        public List<Card> Cards()
        {
            return _context.Cards.ToList();
        }

        [HttpPost("CreateCard")]
        public async Task<IActionResult> CreateCard(Card card)
        {
            if (card == null)
            {
                return BadRequest();
            }

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Connections

        [HttpGet("MyConnections")]
        public List<Connection> Connections(string id)
        {   
            return _context.Connections.Where(x => x.FromId == new Guid(id) || x.ToId == new Guid(id) && x.Status == ConnectionConstants.COMPLETE).ToList();
        }

        [HttpGet("MyPendingConnections")]
        public List<Connection> GetPendingConnections(string id)
        {
            return _context.Connections.Where(x => x.ToId == new Guid(id) && x.Status == ConnectionConstants.PENDING).ToList();
        }

        [HttpPost("CreateConnection")]
        public async Task<IActionResult> CreateConnection([FromForm]string id, [FromForm]string toId)
        {
            if (new Guid(id) == Guid.Empty || new Guid(toId) == Guid.Empty)
            {
                return BadRequest();
            }

            Connection connection = new Connection()
            {
                FromId = new Guid(id),
                ToId = new Guid(toId),
                Status = ConnectionConstants.PENDING
            };

            _context.Connections.Add(connection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("AcceptConnection")]
        public async Task<IActionResult> AcceptConnection(Connection connection)
        {
            if (connection == null)
            {
                return BadRequest();
            }

            connection.Status = ConnectionConstants.COMPLETE;
            _context.Update(connection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("DeleteConnection")]
        public async Task<IActionResult> DeleteConnection(Connection connection)
        {
            if (connection == null)
            {
                return BadRequest();
            }

            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}