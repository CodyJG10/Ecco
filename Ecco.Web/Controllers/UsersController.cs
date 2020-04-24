using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecco.Web.Areas.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecco.Web.Controllers
{
    [Route("Users")]
    [Authorize(Roles = "Moderator,Admin")]
    public class UsersController : Controller
    {
        private UserManager<EccoUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<EccoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        [HttpGet("")]
        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        // GET: Users/AddUserToRole/5/Admin
        [HttpGet("AddUserToRole/{id}/{role}")]
        public async Task<ActionResult> AddUserToRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Index");
        }

        // GET: Users/RemoveFromRole/5/Admin
        [HttpGet("RemoveFromRole/{id}/{role}")]
        public async Task<ActionResult> RemoveFromRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            EccoUser user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}