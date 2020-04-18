using Ecco.Web.Models;
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
        [HttpGet("CreateCompany")]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost("CreateCompany")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCompany([Bind("CompanyName,File,CompanyDescription,OwnerEmail")] CreateCompanyModel model)
        {
            if (ModelState.IsValid)
            {
                //string fileName = model.File.FileName;

                ////Upload template object
                
                //_context.Add(company);
                //await _context.SaveChangesAsync();

                ////Upload image to storage
                //await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(templateModel.File.OpenReadStream());
            }
            return View("Index");
        }
    }
}
