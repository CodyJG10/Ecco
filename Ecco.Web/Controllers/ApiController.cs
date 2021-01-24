using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Entities.Constants;
using Ecco.Entities.Events;
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
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using static Ecco.Web.Models.AuthModel;

namespace Ecco.Web.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EccoUser> _userManager;
        private readonly NotificationService _notifications;
        private readonly IEmailSender _emailSender;
        private readonly EventsHubService _eventsHubService;

        public ApiController(ApplicationDbContext context, UserManager<EccoUser> userManager,
            NotificationService notificationService, 
            IEmailSender emailSender,
            EventsHubService eventsHubService)
        {
            _context = context;
            _userManager = userManager;
            _notifications = notificationService;
            _emailSender = emailSender;
            _eventsHubService = eventsHubService;
        }

        #region Auth

        [HttpGet("UserInfo")]
        public async Task<EccoUser> GetUserInfo()
        {
            var allClaims = User.Claims.ToList();
            var name = allClaims.First(c => c.Type.Contains("nameidentifier")).Value;
            return await _userManager.FindByNameAsync(name);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password and Confirm Password do not match");
                return BadRequest(ModelState);
            }

            EccoUser user = new EccoUser
            {
                UserName = model.Email,
                Email = model.Email,
                ProfileName = model.UserName
            };

            if (_context.Users.Any(x => x.ProfileName == model.UserName))
            {
                ModelState.AddModelError("", "the username " + model.UserName + " is already taken.");
                return BadRequest(ModelState);
            }

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.UserName, "Welcome To Ecco Space!",
                $"Thank you for signing up to Ecco Space! Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return Ok();
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return BadRequest(404);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [HttpGet("UserData")]
        public async Task<string> GetUserData(string id = null, string profileName = null, string email = null)
        {
            if (id != null)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    return JsonConvert.SerializeObject(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            else if (profileName != null)
            {
                var user = _userManager.Users.Single(x => x.ProfileName.Equals(profileName));
                return JsonConvert.SerializeObject(user);
            }
            else
            {
                var user = _userManager.Users.Single(x => x.Email.ToLower().Equals(email.ToLower()));
                return JsonConvert.SerializeObject(user);
            }
        }

        [HttpGet("UserExists")]
        public bool UserExists(string profileName)
        {
            return _userManager.Users.Any(x => x.ProfileName.Equals(profileName));
        }

        [HttpGet("ForgotPassword")]
        public async void ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            string username = model.Username;
            string password = model.Password;
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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

        [HttpGet("ActiveCard")]
        public async Task<string> GetActiveCard(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.ActiveCard;
        }

        [HttpPost("UpdateActiveCard")]
        public async Task<IActionResult> UpdateActiveCard([FromForm]string userId, [FromForm]string cardId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.ActiveCard = cardId;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("ConnectionsWithCard")]
        public int GetConnectionsWithCard(string cardId)
        {
            var card = _context.Cards.Single(x => x.Id == int.Parse(cardId));
            var connectionsWithCard = _context.Connections.Where(x => x.CardId == int.Parse(cardId)).ToList();
            return connectionsWithCard.Count;
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

            var from = await _userManager.FindByIdAsync(id);
            var to = await _userManager.FindByIdAsync(toId);

            _notifications.SendNotification(from.ProfileName + " has sent you their business card!", to);
            UpdateEvent e = new UpdateEvent()
            {
                User = to.UserName,
                Message = "Connection Created"
            };
            _eventsHubService.SendEvent(e);
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

            var from = await _userManager.FindByIdAsync(connection.FromId.ToString());
            var to = await _userManager.FindByIdAsync(connection.ToId.ToString());

            _notifications.SendNotification(to.ProfileName + " has accepted you business card connection!", from);
            return Ok();
        }

        [HttpDelete("DeletePendingConnection")]
        public async Task<IActionResult> DeletePendingConnection(int connectionId)
        {
            try
            {
                var connection = _context.Connections.Single(x => x.Id == connectionId);
                _context.Remove(connection);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateConnectionAndAccept")]
        public async Task<IActionResult> CreateConnectionAndAccept(Connection connection)
        {
            if (connection != null)
            {
                _context.Add(connection);
                await _context.SaveChangesAsync();
                var to = await _userManager.FindByIdAsync(connection.Id.ToString());
                UpdateEvent e = new UpdateEvent()
                {
                    User = to.UserName,
                    Message = "Connection Created"
                };
                _eventsHubService.SendEvent(e);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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
            return _context.Templates.Where(x => x.IsPublic == true).ToList();
        }

        [HttpGet("Template")]
        public Template GetTemplate(int id)
        {
            return _context.Templates.Single(x => x.Id == id);
        }

        #endregion

        #region Company

        [HttpPost("CreateCompany")]
        public async Task<bool> CreateCompany([FromBody]Company company)
        {
            if (company != null)
            {
                _context.Companies.Add(company);

                var userId = company.OwnerId;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                await _userManager.AddToRoleAsync(user, "Company Owner");

                await _emailSender.SendEmailAsync(user.Email, "You Are Now The Owner Of " + company.CompanyName, "Please complete the setup of your company at the following address: https://ecco-space.azurewebsites.net/Company/MyCompany");

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost("InviteEmployee")]
        public async Task<bool> InviteEmployee(EmployeeInvitation model)
        {
            if (model != null)
            {
                _context.EmployeeInvitations.Add(model);
                await _context.SaveChangesAsync();

                var company = _context.Companies.Single(x => x.Id == model.CompanyId);

                var to = await _userManager.FindByIdAsync(model.UserId.ToString());

                _notifications.SendNotification("You have been invited to the company: " + company.CompanyName, to);

                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet("AcceptEmployeeInvitation")]
        public async Task<IActionResult> AcceptEmployeeInvitation(string userId, int companyId)
        {
            if (_context.EmployeeInvitations.Any(x => x.UserId.Equals(new Guid(userId)) && x.CompanyId == companyId))
            {
                var invitation = _context.EmployeeInvitations.Single(x => x.UserId.Equals(new Guid(userId)) && x.CompanyId == companyId);
                invitation.Status = ConnectionConstants.COMPLETE;
                _context.Update(invitation);
                await _context.SaveChangesAsync();

                var company = _context.Companies.Single(x => x.Id == companyId);
                var from = await _userManager.FindByIdAsync(userId);
                var companyOwner = await _userManager.FindByIdAsync(company.OwnerId.ToString());

                _notifications.SendNotification(from.UserName + " has joined your company", companyOwner);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("DenyEmployeeInvitation")]
        public async Task<IActionResult> DenyEmployeeInvitation(string userId, int companyId)
        {
            if (_context.EmployeeInvitations.Any(x => x.UserId.Equals(new Guid(userId)) && x.CompanyId == companyId))
            {
                var invitation = _context.EmployeeInvitations.Single(x => x.UserId.Equals(new Guid(userId)) && x.CompanyId == companyId);
                _context.Remove(invitation);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
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
                var allInvitations = _context.EmployeeInvitations.Where(x => x.UserId == id);

                allInvitations.ToList().ForEach(x =>
                {
                    if (x.Status == ConnectionConstants.COMPLETE)
                    {
                        var company = _context.Companies.Single(company => company.Id == x.CompanyId);
                        companies.Add(company);
                    }
                });
            }

            return companies;
        }

        [HttpGet("MyPendingEmployeeInvites")]
        public List<EmployeeInvitation> GetMyPendingEmployeeInvites(string userId)
        {
            return _context.EmployeeInvitations.Where(x => x.UserId == new Guid(userId) && x.Status == ConnectionConstants.PENDING).ToList();
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

        [HttpPost("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany([FromBody]Company company)
        {
            if (company != null)
            {
                _context.Remove(company);

                if (_context.EmployeeInvitations.Any(x => x.CompanyId == company.Id))
                {
                    foreach (var employee in _context.EmployeeInvitations.Where(x => x.CompanyId == company.Id))
                    {
                        _context.Remove(employee);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return NotFound("The provided company could not be found ");
            }
        }
        #endregion
    }
}