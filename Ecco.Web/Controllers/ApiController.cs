using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Ecco.Web.Areas.Identity;
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
        private readonly UserManager<EccoUser> _userManager;

        public ApiController(ApplicationDbContext context, UserManager<EccoUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("UserInfo")]
        public async Task<EccoUser> GetUserInfo()
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

        [HttpGet("MyCards")]
        public List<Card> GetCards(string id)
        {
            return _context.Cards.Where(x => x.UserId == new Guid(id)).ToList();
        }

        [HttpGet("Card")]
        public Card Card(int id)
        {
            return _context.Cards.Single(x => x.Id == id);
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

        [HttpPost("EditCard")]
        public async Task<IActionResult> EditCard(Card card)
        {
            if (card == null)
            {
                return BadRequest();
            }

            _context.Update(card);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteCard")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var card = _context.Cards.Single(x => x.Id == id);

            if (card == null)
            {
                return BadRequest();
            }

            //Delete all connections containing this card
            _context.Connections.Where(x => x.CardId == id).ToList().ForEach(x => _context.Remove(x));

            _context.Remove(card);
            await _context.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region Connections

        [HttpGet("Connection")]
        public Connection GetConnection(int id)
        {
            return _context.Connections.Single(x => x.Id == id);
        }

        [HttpGet("MyConnections")]
        public List<Connection> Connections(string id)
        {   
            return _context.Connections.Where(x => x.ToId == new Guid(id) && x.Status == ConnectionConstants.COMPLETE).ToList();
        }

        [HttpGet("MyPendingConnections")]
        public List<Connection> GetPendingConnections(string id)
        {
            return _context.Connections.Where(x => x.ToId == new Guid(id) && x.Status == ConnectionConstants.PENDING).ToList();
        }

        [HttpPost("CreateConnection")]
        public async Task<IActionResult> CreateConnection([FromForm]string id, [FromForm]string toId, [FromForm]string cardId)
        {
            if (new Guid(id) == Guid.Empty || new Guid(toId) == Guid.Empty || cardId == null)
            {
                return BadRequest();
            }

            if (_context.Connections.Any(x => x.FromId == new Guid(id))
                && _context.Connections.Any(x => x.ToId == new Guid(toId))
                && _context.Connections.Any(x => x.CardId == int.Parse(cardId)))
            {
                return BadRequest();
            }

            Connection connection = new Connection()
            {
                FromId = new Guid(id),
                ToId = new Guid(toId),
                Status = ConnectionConstants.PENDING,
                CardId = int.Parse(cardId)
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

        #region Templates

        [HttpGet("Templates")]
        public IEnumerable<Template> GetTemplates()
        {
            return _context.Templates.ToList();
        }

        [HttpGet("Template")]
        public Template GetTemplate(int id)
        {
            return _context.Templates.Single(x => x.Id == id);
        }

        #endregion
    }
}