using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly BTAuthContext _context;

        public HomeController(ILogger<HomeController> logger, BTAuthContext context)
        {
            _context = context;
            _logger = logger;           
        }

        public IActionResult Index()
        {
            return View();
            //return RedirectToAction("ShowStats","Tasks");
        }

        public JsonResult GetEvents()
        {
            using (_context)
            {
                var events = _context.Event.ToList();
                
                return new JsonResult(new { data = events });
            }              

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
