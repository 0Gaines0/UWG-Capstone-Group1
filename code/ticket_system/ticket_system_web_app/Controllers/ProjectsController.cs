﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mono.TextTemplating;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;
using Project = ticket_system_web_app.Models.Project;

namespace ticket_system_web_app.Controllers
{
    /// <summary>
    ///     ProjectsController class
    ///     All client method calls require auth token validation.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class ProjectsController : Controller
    {
        #region Fields

        private readonly TicketSystemDbContext _context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ProjectsController(TicketSystemDbContext context)
        {
            _context = context;
        }

        #endregion

        #region View Loaders

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(project => project.ProjectLead).Include(project => project.Collaborators).ThenInclude(collab => collab.Group).FirstOrDefaultAsync(project => project.PId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.Include(project => project.ProjectLead).ToListAsync();

            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCollabRequests()
        {
            IEnumerable<ProjectGroup> result = await this._context.ProjectGroups.Include(collab => collab.Project).Include(collab => collab.Group).Where(collab => !collab.Accepted && collab.Group.ManagerId == ActiveEmployee.Employee.EId).ToListAsync();
            result.OrderBy(collab => collab.Project.PTitle).ThenBy(collab => collab.Group.GName);
            return View(result);
        }

        /// <summary>
        /// Boards the page.
        /// </summary>
        /// <param name="pId">The p identifier.</param>
        /// <returns></returns>
        [HttpGet("Projects/BoardPage/{pId}")]
        public async Task<IActionResult> BoardPage(int pId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectBoard)
                    .ThenInclude(pb => pb.States.OrderBy(s => s.Position))
                        .ThenInclude(s => s.Tasks)
                .Include(p => p.ProjectBoard.States)
                    .ThenInclude(s => s.AssignedGroups)
                        .ThenInclude(ag => ag.Group)
                            .ThenInclude(g => g.Employees)
                .Include(p => p.ProjectBoard.States)
                    .ThenInclude(s => s.AssignedGroups)
                        .ThenInclude(ag => ag.Group)
                            .ThenInclude(g => g.Manager)
                .FirstOrDefaultAsync(p => p.PId == pId);

            if (project == null)
            {
                return NotFound();
            }

            return View("ProjectKanban", project);
        }


        /// <summary>
        /// Edits the kanban.
        /// </summary>
        /// <param name="pId">The p identifier.</param>
        /// <returns></returns>
        [HttpGet("Project/EditKanban/{pId}")]
        public async Task<IActionResult> EditKanban(int pId)
        {
            var board = await _context.Projects.FirstOrDefaultAsync(project => project.PId == pId);
            var project_board = await _context.ProjectBoards
                .Include(b => b.States)
                .ThenInclude(state => state.AssignedGroups)
                .ThenInclude(ag => ag.Group)
                .FirstOrDefaultAsync(b => b.ProjectId == pId);

            var groups = await _context.ProjectGroups
                .Include(pg => pg.Group)
                .Where(pg => pg.ProjectId == pId)
                .Select(pg => pg.Group)
                .ToListAsync();

            ViewBag.Board = board;
            ViewBag.ProjectBoard = project_board;
            ViewBag.Groups = groups;

            return View("EditKanban");
        }

        #endregion

        #region Validated Methods

        /// <summary>
        ///     Creates a project from the specified json request.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="jsonRequest">The json request.</param>
        /// <returns>OK if successful; BadRequest otherwise.</returns>
        [HttpPost("Projects/CreateProject/{authToken}")]
        public async Task<IActionResult> CreateProject(string authToken, [FromBody] CreateProjectRequest jsonRequest)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(CreateProject)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

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

                var project = new Models.Project(lead, jsonRequest.PTitle, jsonRequest.PDescription);
                this._context.Projects.Add(project);
                await this._context.SaveChangesAsync();

                project.Collaborators = this.getCollaboratorsFromCSV(project.PId, String.Join(",", jsonRequest.CollaboratingGroupIDs.Select(x => x.ToString()).ToArray()));
                this._context.Projects.Update(project);
                await this._context.SaveChangesAsync();
                await AddProjectBoardAndDefaultStates(project.PId);

                return Ok(new { message = "Project created successfully", projectID = project.PId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        ///     Returns the details of the project with the specified id.
        /// </summary>
        /// <param name="id">The desired project's ID.</param>
        /// <returns>The project details as a JsonResult, or null if none could be found.</returns>
        [HttpPost("Projects/Details/{authToken}&{id}")]
        public async Task<JsonResult?> Details(string authToken, int id)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(Details)} Got auth token: {authToken}");
                return Json(new { message = "Not logged in." });
            }

            var project = await this._context.Projects.Include(project => project.Collaborators).ThenInclude(collab => collab.Group).Select(project => new
            {
                project.PId,
                project.PTitle,
                project.PDescription,
                ProjectLeadName = this._context.Employees.Where(employee => employee.EId == project.ProjectLeadId).Select(employee => employee.FName + " " + employee.LName).FirstOrDefault(),
                Collaborators = project.Collaborators.Select(collab => new { Accepted = collab.Accepted, GName = collab.Group.GName }).ToList(),
            }).FirstOrDefaultAsync(project => project.PId == id);

            if (project == null)
            {
                return null;
            }

            return Json(project);
        }

        /// <summary>
        ///     Gets the projects related to the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Projects/GetProjectRelatedToEmployee/{authToken}")]
        public async Task<JsonResult> GetProjectRelatedToEmployee(string authToken)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetProjectRelatedToEmployee)} Got auth token: {authToken}");
                return Json(new { success = false, message = "Not logged in." });
            }

            var eId = ActiveEmployee.Employee?.EId;

            if (ActiveEmployee.Employee?.IsAdmin == true)
            {
                var allProjects = await _context.Projects.ToListAsync();

                var allProjectData = allProjects.Select(proj => new
                {
                    PId = proj.PId,
                    PTitle = proj.PTitle
                }).ToList();

                return Json(new { success = true, data = allProjectData });
            }

            var leadProject = await _context.Projects
                .Where(proj => proj.ProjectLeadId == eId)
                .ToListAsync();

            var groupProject = await _context.Projects
                .Include(proj => proj.Collaborators)
                .Where(proj => proj.Collaborators.Any(collab => collab.Group.Employees.Any(employee => employee.EId == eId) || (collab.Group.ManagerId == eId)))
                .ToListAsync();

            var allRelatedProjects = leadProject
                .Concat(groupProject)
                .Distinct()
                .ToList();

            var projectData = allRelatedProjects.Select(proj => new
            {
                PId = proj.PId,
                PTitle = proj.PTitle
            }).ToList();

            return Json(new { success = true, data = projectData });
        }


        /// <summary>
        ///     Edits the specified identifier.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="project">The project.</param>
        /// <param name="csvCollabGroups">The CSV collab groups.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PId,ProjectLeadId,PTitle,PDescription")] Models.Project project, string csvCollabGroups)
        {
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

            project.Collaborators = this.getCollaboratorsFromCSV(id, csvCollabGroups);
            project.ProjectLead = await this._context.Employees.FindAsync(project.ProjectLeadId);

            var projectCurrent = await _context.Projects.Include(p => p.ProjectLead).Include(p => p.Collaborators).FirstOrDefaultAsync(p => p.PId == id);

            if (projectCurrent == null)
            {
                Console.WriteLine("Project not found");
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

                    projectCurrent.Collaborators.Clear();
                    _context.Update(projectCurrent);
                    projectCurrent.Collaborators = project.Collaborators;
                    _context.Update(projectCurrent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.PId))
                    {
                        Console.WriteLine("Project DNE");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return new NoContentResult();
        }

        /// <summary>
        ///     Deletes the confirmed.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///     Accepts the collab request.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptCollabRequest(int projectId, int groupId)
        {
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

            ProjectGroup? collab = this._context.ProjectGroups.FindAsync(projectId, groupId).Result;
            if (collab != null)
            {
                collab.Accepted = true;
                this._context.ProjectGroups.Update(collab);
                await this._context.SaveChangesAsync();
            }
            return new NoContentResult();
        }

        /// <summary>
        ///     Denies the collab request.
        /// </summary>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyCollabRequest(int projectId, int groupId)
        {
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

            ProjectGroup? collab = this._context.ProjectGroups.Include(collab => collab.Project).ThenInclude(project => project.Collaborators).Where(collab => collab.ProjectId == projectId && collab.GroupId == groupId).FirstOrDefaultAsync().Result;
            if (collab != null)
            {
                this._context.ProjectGroups.Remove(collab);
                if (collab.Project.Collaborators.Count <= 1)
                {
                    this._context.Projects.Remove(collab.Project);
                }
                await this._context.SaveChangesAsync();
            }
            return new NoContentResult();
        }

        /// <summary>
        ///     Updates the name of the state.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("Projects/UpdateStateName/{authToken}")]
        public async Task<IActionResult> UpdateStateName(string authToken, [FromBody] UpdateStateNameRequest request)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(UpdateStateName)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

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

        /// <summary>
        ///     Deletes the state.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("Projects/DeleteState/{authToken}")]
        public async Task<IActionResult> DeleteState(string authToken, [FromBody] DeleteStateRequest request)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(DeleteState)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

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

        /// <summary>
        ///     Adds the state.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("Projects/AddState/{authToken}")]
        public async Task<IActionResult> AddState(string authToken, [FromBody] AddStateRequest request)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(AddState)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

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
        ///     Updates the board state order.
        ///     Requires manager perms.
        /// </summary>
        /// <param name="stateOrder">The state order.</param>
        /// <returns></returns>
        [HttpPost("Projects/UpdateBoardStateOrder/{authToken}")]
        public async Task<IActionResult> UpdateBoardStateOrder(string authToken, [FromBody] List<UpdateBoardStateOrderRequest> stateOrder)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(UpdateBoardStateOrder)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }
            if (!ActiveEmployee.IsManager())
            {
                return BadRequest(new { message = "Manager permissions required." });
            }

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

        #endregion

        #region Helpers

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

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.PId == id);
        }

        private ICollection<ProjectGroup> getCollaboratorsFromCSV(int projectId, string csv)
        {
            ICollection<ProjectGroup> result = new List<ProjectGroup>();

            string[] collabIDs = csv.Split(',');

            foreach (string token in collabIDs)
            {
                if (int.TryParse(token, out var collabID))
                {
                    ProjectGroup? collaborator = this._context.ProjectGroups.FindAsync(projectId, collabID).Result;
                    if (collaborator == null)
                    {
                        Project? project = this._context.Projects.FindAsync(projectId).Result;
                        Group? group = this._context.Groups.FindAsync(collabID).Result;

                        collaborator = new ProjectGroup(project, group);
                        collaborator.Accepted = group.ManagerId == ActiveEmployee.Employee.EId;
                    }
                    result.Add(collaborator);
                }
            }

            return result;
        }

        [HttpGet("Projects/GetProjectEmployees/{authToken}&{projectId}&{stateId}")]
        public async Task<IActionResult> GetProjectEmployees(string authToken, int projectId, int stateId)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                return BadRequest(new { message = "Not logged in." });
            }

            var currentUser = ActiveEmployee.Employee;

            var project = await _context.Projects
                .Include(p => p.Collaborators)
                    .ThenInclude(collab => collab.Group)
                        .ThenInclude(group => group.Employees)
                .Include(p => p.Collaborators)
                    .ThenInclude(collab => collab.Group)
                        .ThenInclude(group => group.Manager)
                .FirstOrDefaultAsync(p => p.PId == projectId);

            if (project == null)
            {
                return NotFound(new { message = "Project not found." });
            }
            var isProjectManager = project.Collaborators.Any(c => c.Group?.ManagerId == currentUser.EId);
            var state = project.ProjectBoard?.States.FirstOrDefault(s => s.StateId == stateId);
            if (state == null)
            {
                return NotFound(new { message = "State not found." });
            }

            List<Employee> employees = new();

            if (ActiveEmployee.IsAdmin())
            {
                employees = state.AssignedGroups.Any()
                        ? state.AssignedGroups.SelectMany(ag =>
                        {
                            var groupEmps = ag.Group.Employees ?? new List<Employee>();
                            var manager = ag.Group.Manager;
                            return manager != null ? groupEmps.Append(manager) : groupEmps;
                        }).ToList()
                        : project.Collaborators.SelectMany(c =>
                        {
                            var groupEmps = c.Group.Employees ?? new List<Employee>();
                            var manager = c.Group.Manager;
                            return manager != null ? groupEmps.Append(manager) : groupEmps;
                        }).ToList();
            }
            else if (isProjectManager)
            {
                var groupsInState = state.AssignedGroups
                    .Select(ag => ag.Group)
                    .ToList();

                var managedGroups = groupsInState
                    .Where(g => g.ManagerId == currentUser.EId)
                    .ToList();

                if (managedGroups.Any())
                {
                    employees = managedGroups.SelectMany(g =>
                    {
                        var groupEmps = g.Employees ?? new List<Employee>();
                        return g.Manager != null ? groupEmps.Append(g.Manager) : groupEmps;
                    }).ToList();
                }
                else if (!groupsInState.Any())
                {
                    managedGroups = project.Collaborators
                        .Where(c => c.Group.ManagerId == currentUser.EId)
                        .Select(c => c.Group)
                        .ToList();

                    employees = managedGroups.SelectMany(g =>
                    {
                        var groupEmps = g.Employees ?? new List<Employee>();
                        return g.Manager != null ? groupEmps.Append(g.Manager) : groupEmps;
                    }).ToList();
                }
                else
                {
                    employees = new List<Employee>();
                }
            }
            else
            {
                var assignedGroups = state.AssignedGroups;

                if (!assignedGroups.Any())
                {
                    employees.Add(currentUser);
                }
                else
                {
                    var employeeGroups = project.Collaborators
                        .Where(c => c.Group.Employees.Any(e => e.EId == currentUser.EId))
                        .Select(c => c.Group)
                        .ToList();

                    var assignedGroupIds = assignedGroups.Select(ag => ag.Group.GId).ToHashSet();
                    var isInAssignedGroup = employeeGroups.Any(g => assignedGroupIds.Contains(g.GId));

                    if (isInAssignedGroup)
                    {
                        employees.Add(currentUser);
                    }
                }
            }

            var result = employees
                .GroupBy(e => e.EId)
                .Select(g => g.First())
                .Select(e => new
                {
                    EId = e.EId,
                    Name = $"{e.FName} {e.LName}"
                })
                .ToList();

            return Ok(result);
        }
        #endregion
    }
}