using Microsoft.AspNetCore.Mvc;

using BaF_ToDo.Repositories;
using BaF_ToDo.Models;

[Route("api/[controller]")]
[ApiController]
public class TaskEntityController : ControllerBase
{
    private readonly ITaskEntityRepository _repository;
    public TaskEntityController(ITaskEntityRepository repo)
    {
        _repository = repo;
    }

    // GET: api/Tasks
    [HttpGet]
    public async Task<ActionResult<List<TaskEntityDTO>>> GetTasks()
    {
        var tasks = await _repository.GetAllTasksAsync();

        return tasks.Select(t => t.ToDTO()).ToList();
    }

    // GET: api/TaskEntity/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntityDTO>> GetTaskById(int id)
    {
        var task = await _repository.GetTaskByIdAsync(id);

        if (task == null)
            return NotFound();

        return task.ToDTO();
    }

    // CREATE: api/CreateTask
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TaskEntityDTO>> CreateTask(CreateTaskEntityDTO task)
    {
        var createdTask = await _repository.AddTaskAsync(new TaskEntity{ Title = task.Title});

        var taskDTO = createdTask.ToDTO();

        return CreatedAtAction("GetTaskById", new { id = taskDTO.Id }, taskDTO);
    }

    // UPDATE: api/UpdateTask/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskEntityDTO task)
    {
        var entity = await _repository.GetTaskByIdAsync(id);

        if (entity == null)
            return NotFound();

        entity.Title = task.Title;
        entity.IsCompleted = task.IsCompleted;

        await _repository.UpdateTaskAsync(entity);

        return NoContent();
    }

    // DELETE: api/DeleteTask/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _repository.DeleteTaskAsync(id);

        if (!task)
            return NotFound();

        return NoContent();
    }

    // COMPLETE: api/TaskEntity/5/complete
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        var task = await _repository.GetTaskByIdAsync(id);

        if (task == null)
            return NotFound();

        task.MarkCompleted();

        await _repository.UpdateTaskAsync(task);

        return NoContent();
    }
}
