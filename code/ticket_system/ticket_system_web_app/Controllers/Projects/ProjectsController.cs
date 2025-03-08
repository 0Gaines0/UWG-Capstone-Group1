using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var projects = await _context.Projects.Include(project => project.ProjectLead).ToListAsync();

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

            Employee? lead = await _context.Employees.FindAsync(jsonRequest.PLeadId);
            if (lead == null || string.IsNullOrWhiteSpace(jsonRequest.PTitle) || string.IsNullOrWhiteSpace(jsonRequest.PDescription))
            {
                return BadRequest(new { message = "Invalid project data" });
            }

            ICollection<Group> groups = new List<Group>();
            if (!jsonRequest.CollaboratingGroupIDs.IsNullOrEmpty())
            {
                groups = await _context.Groups.Where(group => jsonRequest.CollaboratingGroupIDs.Contains(group.GId)).ToListAsync();
            }

            var project = new Project(lead, jsonRequest.PTitle, jsonRequest.PDescription, groups);

            this._context.Add(project);
            await this._context.SaveChangesAsync();

            return Ok(new { message = "Project created successfully", projectID = project.PId });
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(i => i.ProjectLead).Include(i => i.AssignedGroups).FirstOrDefaultAsync(i => i.PId == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("PId,ProjectLeadId,PTitle,PDescription")] Project project, string csvCollabGroups)
        {
            if (id != project.PId)
            {
                return NotFound();
            }

            project.AssignedGroups = this.getCollaboratorsFromCSV(csvCollabGroups);
            project.ProjectLead = await this._context.Employees.FindAsync(project.ProjectLeadId);

            var projectCurrent = await _context.Projects.Include(p => p.ProjectLead).Include(p => p.AssignedGroups).FirstOrDefaultAsync(p => p.PId == id);

            if (projectCurrent == null) { 
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    projectCurrent.PTitle = project.PTitle;
                    projectCurrent.PDescription = project.PDescription;
                    projectCurrent.ProjectLeadId = project.ProjectLeadId;
                    projectCurrent.ProjectLead = project.ProjectLead;

                    projectCurrent.AssignedGroups.Clear();
                    _context.Update(projectCurrent);
                    projectCurrent.AssignedGroups = project.AssignedGroups;
                    _context.Update(projectCurrent);
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

        /// <summary>
        /// Returns the details of the project with the specified id.
        /// </summary>
        /// <param name="id">The desired project's ID.</param>
        /// <returns>The project details as a JsonResult, or null if none could be found.</returns>
        public async Task<JsonResult?> Details(int id)
        {
            var project = await this._context.Projects.Include(project => project.AssignedGroups).Select(project => new
            {
                project.PId,
                project.PTitle,
                project.PDescription,
                ProjectLeadName = this._context.Employees.Where(employee => employee.EId == project.ProjectLeadId).Select(employee => employee.FName + " " + employee.LName).FirstOrDefault(),
                project.AssignedGroups
            }).FirstOrDefaultAsync(project => project.PId == id);
            
            if (project == null)
            {
                return null;
            }
            
            return Json(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.PId == id);
        }

        private ICollection<Group> getCollaboratorsFromCSV(string csv)
        {
            ICollection<Group> result = new List<Group>();

            string[] collabIDs = csv.Split(',');

            foreach (string collabID in collabIDs)
            {
                if (int.TryParse(collabID, out var collab))
                {
                    Group? collaborator = this._context.Groups.FindAsync(collab).Result;
                    if (collaborator != null)
                    {
                        result.Add(collaborator);
                    }
                }
            }

            return result;
        }
    }
}
