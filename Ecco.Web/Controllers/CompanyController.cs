using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Ecco.Web.Models;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Controllers
{
    [Route("{controller}")]
    public class CompanyController : Controller
    {
        private ApplicationDbContext _context;
        private StorageService _storage;
        private UserManager<EccoUser> _userManager;

        public CompanyController(ApplicationDbContext context, StorageService storage, UserManager<EccoUser> userManager)
        {
            _context = context;
            _storage = storage;
            _userManager = userManager;
        }

        [HttpGet("CreateCompany")]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [Route("CompanyList")]
        public IActionResult CompanyList()
        {
            var companies = _context.Companies.ToList();
            return View(companies);
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
                    FileName = fileName
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

                //Upload image to storage
                await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(model.File.OpenReadStream());
                return Ok();
            }
            return BadRequest();
        }
    }
}
