using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;


namespace ticket_system_web_app.Controllers
{
    public class ProjectTaskController : Controller
    {
        #region Fields

        private readonly TicketSystemDbContext _context;

        #endregion

        #region Constructors

        public ProjectTaskController(TicketSystemDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Authenticated Methods

        [HttpPost("/Task/Create/{authToken}")]
        public async Task<IActionResult> CreateTask(string authToken, [FromForm] EditProjectTaskRequest model)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(CreateTask)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }

            var task = new ProjectTask();
            task.StateId = model.StateId;
            task.Priority = model.Priority;
            task.Summary = model.Summary;
            task.AssigneeId = model.AssigneeId;
            task.Description = model.Description;

            this._context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/Task/Edit/{authToken}")]
        public async Task<IActionResult> EditTask(string authToken, [FromForm] EditProjectTaskRequest model)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(EditTask)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }

            var task = await _context.Tasks.FindAsync(model.TaskId);
            if (task == null)
            {
                return NotFound();
            }

            var changes = new List<TaskChange>();

            var editor = ActiveEmployee.Employee.EId;

            if (task.StateId != model.StateId)
            {
                var newState = await _context.BoardStates.FirstOrDefaultAsync(s => s.StateId == model.StateId);
                var previousState = await _context.BoardStates.FirstOrDefaultAsync(s => s.StateId == task.StateId);
                changes.Add(new TaskChange
                {
                    Type = "StateChange",
                    PreviousValue = previousState?.StateName.ToString() ?? "Unknown",
                    NewValue = newState?.StateName.ToString() ?? "Unknown",
                    ChangedDate = DateTime.Now,
                    AssigneeId = editor
                });

                task.StateId = model.StateId;
            }

            if (task.Priority != model.Priority)
            {
                changes.Add(new TaskChange
                {
                    Type = "PriorityChange",
                    PreviousValue = this.getPriority(task.Priority),
                    NewValue = this.getPriority(model.Priority),
                    ChangedDate = DateTime.Now,
                    AssigneeId = editor
                });

                task.Priority = model.Priority;
            }

            if (task.Summary != model.Summary)
            {
                changes.Add(new TaskChange
                {
                    Type = "SummaryChange",
                    PreviousValue = task.Summary,
                    NewValue = model.Summary,
                    ChangedDate = DateTime.Now,
                    AssigneeId = editor
                });

                task.Summary = model.Summary;
            }

            if (task.Description != model.Description)
            {
                changes.Add(new TaskChange
                {
                    Type = "DescriptionChange",
                    PreviousValue = task.Description,
                    NewValue = model.Description,
                    ChangedDate = DateTime.Now,
                    AssigneeId = editor
                });

                task.Description = model.Description;
            }

            if (task.AssigneeId != model.AssigneeId)
            {
                var previousAssignee = await _context.Employees.FirstOrDefaultAsync(x => x.EId == task.AssigneeId);
                var newAssignee = await _context.Employees.FirstOrDefaultAsync(x => x.EId == model.AssigneeId);

                changes.Add(new TaskChange
                {
                    Type = "AssigneeChange",
                    PreviousValue = previousAssignee != null
                    ? $"{previousAssignee.FName} {previousAssignee.LName}"
                    : "Unassigned",
                    NewValue = newAssignee != null
                    ? $"{newAssignee.FName} {newAssignee.LName}"
                    : "Unassigned",
                    ChangedDate = DateTime.Now,
                    AssigneeId = editor
                });

                task.AssigneeId = model.AssigneeId;
            }

            if (changes.Any())
            {
                await _context.TaskChanges.AddRangeAsync(changes);
                await _context.SaveChangesAsync();

                var changeLogs = changes.Select(c => new TaskChangeLog
                {
                    TaskId = task.TaskId,
                    ChangeId = c.ChangeId
                });

                await _context.TaskChangeLogs.AddRangeAsync(changeLogs);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("/Task/Delete/{authToken}&{taskId}")]
        public async Task<IActionResult> DeleteTask(string authToken, int taskId)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(DeleteTask)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("/Task/History/{authToken}&{taskId}")]
        public async Task<IActionResult> GetTaskHistory(string authToken, int taskId)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                Console.WriteLine($"{nameof(GetTaskHistory)} Got auth token: {authToken}");
                return BadRequest(new { message = "Not logged in." });
            }

            var changes = await _context.TaskChangeLogs
                .Where(log => log.TaskId == taskId)
                .Include(log => log.TaskChange)
                    .ThenInclude(tc => tc.Assignee)
                .Select(log => new
                {
                    type = log.TaskChange.Type,
                    changedDate = log.TaskChange.ChangedDate,
                    previousValue = log.TaskChange.PreviousValue,
                    newValue = log.TaskChange.NewValue,
                    assigneeName = log.TaskChange.Assignee != null ? $"{log.TaskChange.Assignee.FName} {log.TaskChange.Assignee.LName}" : "System"
                })
                .OrderByDescending(c => c.changedDate)
                .ToListAsync();

            return Json(changes);
        }

        [HttpPost("/Task/AddComment/{authToken}&{taskId}")]
        public async Task<IActionResult> AddComment(string authToken, int taskId, [FromForm] string commentText)
        {
            if (!ActiveEmployee.IsValidRequest(authToken))
            {
                return BadRequest(new { message = "Not logged in." });
            }

            if (string.IsNullOrWhiteSpace(commentText))
            {
                return BadRequest(new { message = "Comment text cannot be empty." });
            }

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            var comment = new TaskComment
            {
                TaskId = taskId,
                CommenterId = ActiveEmployee.Employee.EId,
                CommentText = commentText,
                CommentedAt = DateTime.Now
            };

            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment added successfully." });
        }

        [HttpGet("/Task/Comments/{taskId}")]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            var comments = await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .Include(c => c.Commenter)
                .OrderByDescending(c => c.CommentedAt)
                .Select(c => new {
                    commenter = c.Commenter.FName + " " + c.Commenter.LName,
                    text = c.CommentText,
                    date = c.CommentedAt.ToString("g")
                })
                .ToListAsync();

            return Json(comments);
        }



        #endregion

        #region Helper Methods

        private String getPriority(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                default:
                    return "Medium";
            }
        }

        #endregion
    }
}
