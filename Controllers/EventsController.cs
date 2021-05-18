using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using BugTracker.Areas.Identity.Data;
using System.Diagnostics;

namespace BugTracker.Controllers
{
    public class EventsController : Controller
    {
        private readonly BTAuthContext _context;
        private readonly UserManager<BugTrackerUser> _userManager;

        public EventsController(BTAuthContext context, UserManager<BugTrackerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public JsonResult GetEvents()
        {
            using (_context)
            {                
                var events = _context.Event.Where(e=>e.UserID == _userManager.GetUserId(User)).ToList();
                return new JsonResult(new { data = events });
            }
        }

               
        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (_context)
            {

                if (e.EventId > 0)
                {
                    //Update the event
                    var singleEvent = _context.Event.Where(a => a.EventId == e.EventId).FirstOrDefault();
                    if (singleEvent != null)
                    {
                        singleEvent.UserID = _userManager.GetUserId(User);
                        singleEvent.Subject = e.Subject;
                        singleEvent.Start = e.Start;
                        singleEvent.Stop = e.Stop;
                        singleEvent.Description = e.Description;
                        singleEvent.IsFullDay = e.IsFullDay;
                        singleEvent.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    e.UserID = _userManager.GetUserId(User);
                    _context.Event.Add(e);
                }
                _context.SaveChanges();
                status = true;
            }
            return new JsonResult(new { status = status });
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (_context)
            {
                var v = _context.Event.Where(a => a.EventId == eventID).FirstOrDefault();
                if (v != null)
                {
                    _context.Event.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult(new { data = new { status = status } });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            using (_context)
            {
                var events = await _context.Event.Where(e => e.UserID == _userManager.GetUserId(User)).ToListAsync();
                //return new JsonResult(new { data = events });
                return View();
            }
            
        }

        //// GET: Events/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var @event = await _context.Event
        //        .FirstOrDefaultAsync(m => m.EventId == id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(@event);
        //}
       
        //// POST: Events/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("EventId,UserID,Subject,Description,Start,Stop,ThemeColor,IsFullDay")] Event @event)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        @event.UserID=_userManager.GetUserId(User);
        //        _context.Add(@event);
        //        await _context.SaveChangesAsync();
        //        //return RedirectToAction(nameof(Index));
        //        return RedirectToAction("Index", "Events");
        //    }
            
        //    return RedirectToAction("Index","Events");
        //}
        

        //// POST: Events/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("EventId,UserID,Subject,Description,Start,Stop,ThemeColor,IsFullDay")] Event @event)
        //{
        //    if (id != @event.EventId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(@event);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EventExists(@event.EventId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index", "Events");
        //    }
        //    return View(@event);
        //}

        
        //// POST: Events/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var @event = await _context.Event.FindAsync(id);
        //    _context.Event.Remove(@event);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Events");
        //}

        //private bool EventExists(int id)
        //{
        //    return _context.Event.Any(e => e.EventId == id);
        //}
    }
}
