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
            var projects = await _context.Projects.Include(project => project.ProjectLead).ToListAsync();

            return View(projects);
        }

        [HttpGet("Projects/BoardPage/{pId}")]
        public async Task<IActionResult> BoardPage(int pId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.PId == pId);
            return View("ProjectKanban", project);
        }

        [HttpGet("Project/EditKanban/{pId}")]
        public async Task<IActionResult> EditKanban(int pId)
        {
            var board = await _context.Projects.FirstOrDefaultAsync(project => project.PId == pId);
            var project_board = await _context.ProjectBoards.Include(b => b.States).FirstOrDefaultAsync(b => b.ProjectId == pId);

            ViewBag.Board = board;
            ViewBag.ProjectBoard = project_board;

            return View("EditKanban");
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

        [HttpPost]
        public async Task<IActionResult> UpdateStateName([FromBody] UpdateStateNameRequest request)
        {
            if (request == null || request.Id <= 0 || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            try
            {
                var state = await this._context.BoardStates.FindAsync(request.Id);
                if (state == null)
                {
                    return NotFound(new { message = "State not found." });
                }
                state.StateName = request.Name;
                await _context.SaveChangesAsync();

                return Ok(new { message = "State name updated successfully.", updatedName = state.StateName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating state name.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteState([FromBody] DeleteStateRequest request)
        {
            if (request == null || request.Id <= 0)
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            try
            {
                var state = await _context.BoardStates.FindAsync(request.Id);
                if (state == null)
                {
                    return NotFound(new { message = "State not found." });
                }

                _context.BoardStates.Remove(state);
                await _context.SaveChangesAsync();

                return Ok(new { message = "State deleted successfully.", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting state.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddState([FromBody] AddStateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || request.BoardId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid request data." });
            }

            try
            {
                int maxPosition = await _context.BoardStates
                    .Where(s => s.BoardId == request.BoardId)
                    .MaxAsync(s => (int?)s.Position) ?? 0;

                var newState = new BoardState
                {
                    StateName = request.Name,
                    BoardId = request.BoardId, 
                    Position = maxPosition + 1 
                };

                _context.BoardStates.Add(newState);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, stateId = newState.StateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error adding state.", error = ex.Message });
            }
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

            try
            {
                Employee lead = await _context.Employees.FindAsync(jsonRequest.PLeadId);
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

                await _context.AddAsync(project);
                await _context.SaveChangesAsync();  
                await AddProjectBoardAndDefaultStates(project.PId);

                return Ok(new { message = "Project created successfully", projectID = project.PId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        private async Task AddProjectBoardAndDefaultStates(int projectId)
        {
            try
            {
                    var board = new ProjectBoard { ProjectId = projectId };
                    await _context.ProjectBoards.AddAsync(board);
                    await _context.SaveChangesAsync(); 

                    board = await _context.ProjectBoards.FirstOrDefaultAsync(b => b.ProjectId == projectId);

                    if (board == null)
                    {
                        Console.Out.WriteLine("Failed to create ProjectBoard.");
                        return;
                    }

                    var boardStates = new List<BoardState>
            {
                new BoardState { BoardId = board.BoardId, StateName = "To Do", Position = 1, ProjectBoard = board },
                new BoardState { BoardId = board.BoardId, StateName = "In Progress", Position = 2, ProjectBoard = board },
                new BoardState { BoardId = board.BoardId, StateName = "Completed", Position = 3, ProjectBoard = board }
            };

                    await _context.BoardStates.AddRangeAsync(boardStates);
                    var result = await _context.SaveChangesAsync();

                    Console.Out.WriteLine($"Saved {result} BoardStates successfully.");
               
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Error in AddProjectBoardAndDefaultStates: {ex.Message}");
            }
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

        [HttpPost]
        public async Task<IActionResult> UpdateBoardStateOrder([FromBody] List<UpdateBoardStateOrderRequest> stateOrder)
        {
            if (stateOrder == null || !stateOrder.Any())
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            try
            {
                var stateIds = stateOrder.Select(s => s.StateId).ToList();
                var boardStates = await _context.BoardStates
                    .Where(bs => stateIds.Contains(bs.StateId))
                    .ToListAsync();

                foreach (var item in stateOrder)
                {
                    var state = boardStates.FirstOrDefault(s => s.StateId == item.StateId);
                    if (state != null)
                    {
                        state.Position = item.Position;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Board state order updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating board state order.", error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetProjectRelatedToEmployee()
        {
            var eId = ActiveEmployee.Employee?.EId;
            var leadProject = await this._context.Projects.Where(proj => proj.ProjectLeadId == eId).ToListAsync();
            var groupProject = await this._context.Projects.Where(proj => proj.AssignedGroups.Any(group => group.Employees.Any(employee => employee.EId == eId))).ToListAsync();
            var allProjects = leadProject.Concat(groupProject).Distinct().ToList();

            var projectData = allProjects.Select(proj => new
            {
                PId = proj.PId,
                PTitle = proj.PTitle
            }).ToList();
            return Json(new { success = true, data = projectData });
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
    }
}
