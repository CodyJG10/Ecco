using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecco.Entities;
using Ecco.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace Ecco.Web.Controllers
{
    [Route("Cards")]
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("{id}")]
        public ActionResult GetCard(int id)
        {
            var card = _context.Cards.Single(x => x.Id == id);
            return View("Card", card);
        }
    }
}