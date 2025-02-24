using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;

namespace ticket_system_web_app.Controllers.Projects
{
    public class ProjectsController : Controller
    {
        private readonly TicketSystemDbContext _context;

        public ProjectsController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of all collaborators on the project with the specified ID. If no such project exists, returns null.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <param name="id">The project ID.</param>
        /// <returns>The list of collaborators.</returns>
        public IEnumerable<Group>? GetCollaboratorsOn(int? id)
        {
            if (id == null)
            {
                return null;
            }

            IEnumerable<Group> result = new List<Group>();
            foreach (var g in _context.Groups)
            {
                foreach (var p in g.AssignedProjects)
                {
                    if (p.PId == id)
                    {
                        result.Append(g);
                    }
                }
            }

            return result;
        }

        public IActionResult Back()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.ToListAsync();

            return View(projects);
        }

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
        public async Task<IActionResult> Create([Bind("PId,ProjectLeadId,PTitle,PDescription")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Group> groups = await _context.Groups.ToListAsync();
            ViewData["Groups"] = groups;
            return View(project);
        }

        /// <summary>
        /// Creates a project from the specified json request.
        /// </summary>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>OK if successful; BadRequest otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest jsonRequest)
        {
            if (jsonRequest == null)
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            if (jsonRequest.PLeadId == 0 || string.IsNullOrWhiteSpace(jsonRequest.PTitle) || string.IsNullOrWhiteSpace(jsonRequest.PDescription))
            {
                return BadRequest(new { message = "Invalid project data" });
            }

            //Apparently this is enough to make the project appear on the server; _context.Projects.Add(project); is unnecessary.
            var project = new Project(jsonRequest.PLeadId, jsonRequest.PTitle, jsonRequest.PDescription);

            if (!jsonRequest.CollaboratingGroupIDs.IsNullOrEmpty())
            {
                var groups = await _context.Groups.Where(group => jsonRequest.CollaboratingGroupIDs.Contains(group.GId)).ToListAsync();
                project.AssignedGroups = groups;
            }


            await _context.SaveChangesAsync();

            return Ok(new { message = "Project created successfully", projectID = project.PId });
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PId,ProjectLeadId,PTitle,PDescription")] Project project)
        {
            if (id != project.PId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.PId))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.PId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.PId == id);
        }
    }
}
