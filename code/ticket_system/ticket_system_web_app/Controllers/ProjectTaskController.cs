using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
using ticket_system_web_app.Models;
using ticket_system_web_app.Models.RequestObj;


namespace ticket_system_web_app.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly TicketSystemDbContext _context;

        public ProjectTaskController(TicketSystemDbContext context)
        {
            this._context = context;
        }

        [HttpPost("/Task/Edit")]
        public async Task<IActionResult> EditTask([FromForm] EditProjectTaskRequest model)
        {
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


            [HttpDelete("/Task/Delete/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("/Task/History/{taskId}")]
        public async Task<IActionResult> GetTaskHistory(int taskId)
        {
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


    }
}
