using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Entities.Constants;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Ecco.Web.Models;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Controllers
{
    [Route("Company")]
    public class CompanyController : Controller
    {
        private ApplicationDbContext _context;
        private StorageService _storage;
        private UserManager<EccoUser> _userManager;
        private NotificationService _notifications;

        public CompanyController(ApplicationDbContext context, StorageService storage, UserManager<EccoUser> userManager, NotificationService notifications)
        {
            _context = context;
            _storage = storage;
            _userManager = userManager;
            _notifications = notifications;
        }

        [Route("")]
        [Route("Index")]
        [Route("MyCompany")]
        [Authorize(Roles = "Company Owner")]
        public IActionResult MyCompany()
        {
            return View();
        }

        [HttpGet("EditCompany")]
        [Authorize(Roles = "Company Owner")]
        public async Task<IActionResult> EditCompany()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var company = _context.Companies.Single(x => x.OwnerId.ToString() == user.Id);
            EditCompanyModel model = new EditCompanyModel()
            {
                CompanyName = company.CompanyName,
                CompanyDescription = company.CompanyDescription
            };
            return View("EditCompanyDetails", model);
        }

        [HttpPost("EditCompany")]
        [Authorize(Roles = "Company Owner")]
        public async Task<IActionResult> EditCompany([FromForm] EditCompanyModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var company = _context.Companies.Single(x => x.OwnerId.ToString() == user.Id);
            company.CompanyName = model.CompanyName;
            company.CompanyDescription = model.CompanyDescription;
            _context.Update(company);
            await _context.SaveChangesAsync();
            return View("MyCompany");
        }

        [Route("CreateTemplate")]
        [Authorize(Roles = "Company Owner")]
        public IActionResult CreateTemplate()
        {
            return View();
        }

        [HttpPost("CreateTemplate")]
        [Authorize(Roles =  "Company Owner")]
        public async Task<IActionResult> CreateTemplate([FromForm] EditCompanyModel model)
        {

            if (model.File != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var company = _context.Companies.Single(x => x.OwnerId.ToString() == user.Id);

                string fileName = Guid.NewGuid().ToString();

                if (company.TemplateId != 0)
                {
                    var oldTemplate = _context.Templates.Single(x => x.Id == company.TemplateId);

                    fileName = oldTemplate.FileName;

                    await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(model.File.OpenReadStream());

                    _context.Templates.Update(oldTemplate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //Upload template object
                    Template template = new Template()
                    {
                        FileName = fileName,
                        Title = company.CompanyName,
                        IsPublic = false
                    };

                    _context.Add(template);
                    await _context.SaveChangesAsync();

                    //Upload image to storage
                    await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(model.File.OpenReadStream());

                    var newTemplate = _context.Templates.Single(x => x.FileName == fileName && x.Title == company.CompanyName);
                    company.TemplateId = newTemplate.Id;

                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
            }

            return View("MyCompany");
        }

        [HttpGet("InviteEmployee")]
        [Authorize(Roles = "Company Owner")]
        public IActionResult InviteEmployeeToCompany()
        {
            return View();
        }

        [HttpPost("InviteEmployee")]
        [Authorize(Roles = "Company Owner")]
        public async Task<IActionResult> InviteEmployeeToCompany(InviteEmployeeModel model)
        {
            if (model != null)
            {
                var userToInvite = await _userManager.FindByNameAsync(model.Username);
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                Company company = _context.Companies.Single(x => x.OwnerId == new Guid(currentUser.Id));

                if (_context.EmployeeInvitations.Any(x => x.UserId == new Guid(userToInvite.Id) && x.CompanyId == company.Id))
                {
                    return Content("This user has already been invited to " + company.CompanyName);
                }

                EmployeeInvitation invitationModel = new EmployeeInvitation()
                {
                    CompanyId = company.Id,
                    Status = ConnectionConstants.PENDING,
                    UserId = new Guid(userToInvite.Id)
                };

                _context.Add(invitationModel);
                await _context.SaveChangesAsync();

                _notifications.SendNotification("You have a new company invite!", await _userManager.FindByNameAsync(model.Username));

                return Content("Succesfully invited user to " + company.CompanyName);
            }
            else
            {
                return Content("No input detected");
            }
        }

        [HttpGet("CreateCompany")]
        [Authorize()]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost("CreateCompany")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany(CreateCompanyModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = Guid.NewGuid().ToString();
                var userId = new Guid((await _userManager.FindByNameAsync(model.OwnerEmail)).Id);


                Template template = new Template()
                {
                    Title = model.CompanyName,
                    FileName = fileName,
                    IsPublic = false,
                };

                _context.Add(template);
                await _context.SaveChangesAsync();

                int templateId = _context.Templates.Single(x => x.FileName.Equals(fileName)).Id;


                Company company = new Company()
                {
                    CompanyDescription = model.CompanyDescription,
                    CompanyName = model.CompanyName,
                    OwnerId = userId,
                    TemplateId = templateId
                };

                _context.Add(company);
                await _context.SaveChangesAsync();

                var companyId = _context.Companies.Single(x => x.OwnerId == userId).Id;
                EmployeeInvitation invitation = new EmployeeInvitation()
                {
                    CompanyId = companyId,
                    Status = ConnectionConstants.COMPLETE,
                    UserId = userId
                };

                _context.Add(invitation);
                await _context.SaveChangesAsync();

                //Upload image to storage
                await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(model.File.OpenReadStream());
                return Ok();
            }
            return BadRequest();
        }

        [Authorize("Admin")]
        [Route("CompanyList")]
        public IActionResult CompanyList()
        {
            var companies = _context.Companies.ToList();
            return View(companies);
        }
    }
}
