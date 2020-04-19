﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Entities.Constants;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Ecco.Web.Models;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly NotificationService _notifications;

        public ApiController(ApplicationDbContext context, UserManager<EccoUser> userManager, NotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notifications = notificationService;
        }

        #region Auth

        [HttpGet("UserInfo")]
        public async Task<EccoUser> GetUserInfo()
        {
            var allClaims = User.Claims.ToList();
            var name = allClaims.First(c => c.Type.Contains("nameidentifier")).Value;
            return await _userManager.FindByNameAsync(name);
        }

        #endregion

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

        #region Notifications

        //public void SendUserNotification()
        //{ 

        //}

        #endregion

        #region Company

        [HttpPost("CreateCompany")]
        public async Task<bool> CreateCompany([FromBody]Company company)
        {
            if (company != null)
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost("InviteEmployee")]
        public async Task<bool> InviteEmployee([FromForm]EmployeeInvitation model)
        {
            if (model != null)
            {
                _context.EmployeeInvitations.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet("Company")]
        public Company GetCompany(int id)
        {
            var company = _context.Companies.Single(x => x.Id == id);
            return company;
        }

        [HttpGet("MyOwnedCompany")]
        public async Task<Company> GetMyOwnedCompany(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            Guid id = new Guid(userId);
            if (_context.Companies.Any(x => x.OwnerId == id))
            {
                return _context.Companies.Single(x => x.OwnerId == id);
            }
            else
            {
                return null;
            }
        }

        [HttpGet("MyEmployers")]
        public List<Company> GetMyEmployers(string userId)
        {
            Guid id = new Guid(userId);
            List<Company> companies = new List<Company>();
            if (_context.EmployeeInvitations.Any(x => x.UserId == id))
            {
                foreach (var invitation in _context.EmployeeInvitations.Where(x => x.UserId == id))
                {
                    if (invitation.Status == ConnectionConstants.COMPLETE)
                    {
                        var company = _context.Companies.Single(x => x.Id == invitation.CompanyId);
                        companies.Add(company);
                    }
                }
            }
            return companies;
        }

        [HttpPost("LeaveCompany")]
        public async Task<bool> LeaveCompany([FromBody]Company company, string userId)
        {
            if (company != null && userId != null)
            {
                var employeeInvitation = _context.EmployeeInvitations.Single(x => x.CompanyId == company.Id && x.UserId == new Guid(userId));
                _context.EmployeeInvitations.Remove(employeeInvitation);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}