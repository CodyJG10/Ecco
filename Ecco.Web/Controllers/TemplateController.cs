using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecco.Entities;
using Ecco.Web.Data;
using Ecco.Web.Services;
using System.IO;
using Microsoft.Azure.Storage.Blob;
using Ecco.Web.Models;

namespace Ecco.Web.Controllers
{
    [Route("Templates")]
    public class TemplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly StorageService _storage;

        public TemplateController(ApplicationDbContext context, StorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        // GET: Template
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Templates.ToListAsync());
        }

        // GET: Template/Details/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Template/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("File,Title")] CreateTemplateModel templateModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = templateModel.File.FileName;
             
                //Upload template object
                Template template = new Template()
                {
                    FileName = fileName,
                    Title = templateModel.Title
                };
                _context.Add(template);
                await _context.SaveChangesAsync();

                //Upload image to storage
                await _storage.CloudBlobClient.GetContainerReference("templates").GetBlockBlobReference(fileName).UploadFromStreamAsync(templateModel.File.OpenReadStream());

                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }

        // GET: Template/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }
            return View(template);
        }

        // POST: Template/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Files,FileName,Title")] Template template)
        {
            if (id != template.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(template);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateExists(template.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(template);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //[HttpGet("Template")]
        //public async byte[] GetTemplate(string fileName)
        //{
        //    var container = _storage.CloudBlobClient.GetContainerReference("templates");
        //    var blob = container.GetBlockBlobReference(fileName);
        //    Directory.CreateDirectory("assets/templates");
        //    await blob.DownloadToFileAsync("assets/templates/" + fileName, FileMode.OpenOrCreate);
        //    return blob.DownloadToByteArray();
        //}

        [HttpGet("Templates")]
        public async Task<List<byte[]>> GetAllTemplates()
        {
            List<byte[]> blobs = new List<byte[]>();
            BlobContinuationToken blobContinuationToken = null;
            var container = _storage.CloudBlobClient.GetContainerReference("templates");
            do
            {
                var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    Console.WriteLine(item.Uri);
                    string fileName = item.Uri.Segments[item.Uri.Segments.Length - 1];
                    var blob = container.GetBlockBlobReference(fileName);
                    Directory.CreateDirectory("assets/templates");
                    await blob.DownloadToFileAsync("assets/templates/" + fileName, FileMode.Create);
                    byte[] blobData = System.IO.File.ReadAllBytes("assets/templates/" + fileName);
                    blobs.Add(blobData);
                }
            } while (blobContinuationToken != null);
            return blobs;
        }

        private bool TemplateExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
