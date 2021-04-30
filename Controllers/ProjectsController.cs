using BugTracker.Areas.Identity.Data;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly BTAuthContext _context;
        private readonly UserManager<BugTrackerUser> _userManager;
        

        public ProjectsController(UserManager<BugTrackerUser> userManager, BTAuthContext context)
        {
            _userManager = userManager;            
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Where(p=>p.UserID== _userManager.GetUserId(User)).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["GetProjects"] = searchString;
            var projRes = await _context.Projects.Where(p => p.UserID == _userManager.GetUserId(User)).ToListAsync();
            

            if (!string.IsNullOrEmpty(searchString))
            {
                projRes = projRes.Where(p =>
                    p.ProjectName.Contains(searchString) ||
                    p.ProjectDescription.Contains(searchString) ||
                    p.ProjectDate.Contains(searchString)                    
                ).ToList();                         
            }
            
            return View(projRes);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.Where(t => t.ProjectID == id).ToListAsync();
            var projects = await _context.Projects.FirstOrDefaultAsync(t => t.ProjectID == id);            
           
            var tuple = (tasks, projects);
            return View(tuple);
            
        }

        //public async Task<IActionResult> DetailsGraph(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var tasks = await _context.Tasks.Where(t => t.ProjectID == id).ToListAsync();
        //    var projects = await _context.Projects.FirstOrDefaultAsync(t => t.ProjectID == id);
        //    var tasksCompleted = tasks.Where(t => t.Completed == true);
        //    var tasksUncompleted = tasks.Where(t => t.Completed == false);
        //    var jsonRes = new JsonResult(new { myComplete = tasksCompleted, myIncomplete = tasksUncompleted });

        //    return jsonRes;

        //}


        // GET: Projects/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BugTrackerUser bt, [Bind("ProjectID,UserID,ProjectName,ProjectDescription,ProjectDate")] Projects projects)
        {

            if (ModelState.IsValid)
            {
                projects.UserID = _userManager.GetUserId(User);
                _context.Add(projects);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projects);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,UserID,ProjectName,ProjectDescription,ProjectDate")] Projects projects)
        {
            if (id != projects.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    projects.UserID = _userManager.GetUserId(User);
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.ProjectID))
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
            return View(projects);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
