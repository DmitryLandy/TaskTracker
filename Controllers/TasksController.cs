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
using Microsoft.AspNetCore.Authorization;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly BTAuthContext _context;
        private readonly UserManager<BugTrackerUser> _userManager;

        public TasksController(UserManager<BugTrackerUser> userManager, BTAuthContext context)
        {
            _userManager = userManager;
            _context = context;
        }
       

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projRes = await _context.Projects.Where(p => p.UserID == _userManager.GetUserId(User)).ToListAsync();
            var taskRes = await _context.Tasks.ToListAsync();
            var filteredTask = taskRes.Join(projRes,
                      t => t.ProjectID,
                      p => p.ProjectID,
                      (t, p) => t).ToList();            

            var tuple = (filteredTask, projRes);
            return View(tuple);
        }
        [HttpPost]
        public async Task<IActionResult> Index(String searchString, bool? comp)
        {
            ViewData["GetTasks"] = searchString;            
            
            var projRes = await _context.Projects.Where(p => p.UserID == _userManager.GetUserId(User)).ToListAsync();
            var taskRes = await _context.Tasks.ToListAsync();
            var filteredTask = taskRes.Join(projRes,
                      t => t.ProjectID,
                      p => p.ProjectID,
                      (t, p) => t).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                filteredTask = filteredTask.Where(t =>
                    t.TaskName.Contains(searchString) ||
                    t.TaskDescription.Contains(searchString) ||
                    t.TaskDeadline.Contains(searchString) ||
                    t.TaskStartDate.Contains(searchString)
                ).ToList();
            }
            if (comp.HasValue)
            {
                filteredTask = filteredTask.Where(t =>
                    t.Completed == comp
                ).ToList();
            }

            var tuple = (filteredTask, projRes);
            return View(tuple);
        }

               
        public async Task<IActionResult> ShowStats()
        {
            TaskViewModel tvm = new TaskViewModel();

            tvm.projRes = await _context.Projects.Where(p => p.UserID == _userManager.GetUserId(User)).ToListAsync();
            tvm.taskRes = await _context.Tasks.ToListAsync();
            tvm.taskResFiltered = tvm.taskRes.Join(tvm.projRes,
                      t => t.ProjectID,
                      p => p.ProjectID,
                      (t, p) => t).ToList();

            tvm.tasksCompleted= tvm.taskResFiltered.Where(t => t.Completed == true).Count();
            tvm.tasksUncompleted = tvm.taskResFiltered.Where(t => t.Completed != true).Count();
            tvm.taskCount = tvm.taskResFiltered.Count();
            tvm.projCount = tvm.projRes.Count();

            return View("GetStats",tvm);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // GET: Tasks/Create
        public IActionResult Create(int? projID)
        {
            return View(projID);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskID,ProjectID,TaskName,TaskDescription,TaskStartDate,TaskDeadline,Completed")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskID,ProjectID,TaskName,TaskDescription,TaskStartDate,TaskDeadline,Completed")] Tasks tasks)
        {
            if (id != tasks.TaskID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.TaskID))
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
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskID == id);
        }
    }
}
