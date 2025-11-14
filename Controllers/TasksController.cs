using Microsoft.AspNetCore.Mvc;
using ToDoAppAPI.Models;
using ToDoAppAPI.Services;

namespace ToDoAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service; // Se inyecta el servicio

        public TasksController(TaskService service) // Se recibe el servicio
        {
            _service = service;
        }

        // ========================================
        // GET /api/tasks.
        // ========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetTasks()
        {
            var tasks = await _service.GetAllAsync();
            return Ok(tasks);
        }

        // ========================================
        // GET /api/tasks/{id}
        // ========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetTask(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        // ========================================
        // POST /api/tasks
        // ========================================
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> CreateTask(ToDoTask task)
        {
            var created = await _service.CreateAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
        }

        // ========================================
        // PUT /api/tasks/{id}
        // ========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTask updated)
        {
            if (id != updated.Id)
                return BadRequest("El ID no coincide con el de la URL.");

            var result = await _service.UpdateAsync(updated);

            if (result == null)
                return NotFound("No se encontro la tarea a actualizar.");

            return Ok(result);
        }

        // ========================================
        // DELETE /api/tasks/{id}
        // ========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // ========================================
        // PATCH /api/tasks/{id}/toggle
        // ========================================
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTask(int id)
        {
            var task = await _service.ToggleAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        // ========================================
        // POST /api/tasks/welcome
        // ========================================
        [HttpPost("welcome")]
        public async Task<IActionResult> LoadWelcome()
        {
            var task = await _service.LoadWelcomeAsync();
            if (task == null)
                return BadRequest("Ya existen tareas en el servidor");
            return Ok(task);
        }

        // ========================================
        // POST /api/tasks/all
        // ========================================
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllTasks()
        {
            await _service.DeleteAllTasksAsync();
            return NoContent();
        }

        // ========================================
        // POST /api/tasks/test
        // ========================================
        [HttpPost("test")]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> LoadTestNotes()
        {
            var tasks = await _service.LoadTestTasksAsync();
            return Ok(tasks);
        }
    }
}