using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticket_system_web_app.Data;
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
            if (task == null) {
                return NotFound();
            }
            task.StateId = model.StateId;
            task.Priority = model.Priority;
            task.Summary = model.Summary;
            task.Description = model.Description;
            task.AssigneeId = model.AssigneeId;

            await _context.SaveChangesAsync();
            return Ok();
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

    }
}
