using webapi.Model;
using webapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class TasksController(ITaskService service) : ControllerBase
{

    /// <summary>
    /// Henter alle oppgaver som er åpne og har en forfallsdato som er passert.
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetOverdue()
    {
        return Ok();
    }

    /// <summary>
    /// Henter alle oppgaver med en spesifikk status.
    /// </summary>
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetByStatus(
        TaskItemStatus status)
    {
        var tasks = await service.GetAllAsync();

        var filteredTasks = tasks
            .Where(task => task.Status == status);

        return Ok(filteredTasks);
    }

    /// <summary>
    /// Oppdaterer statusen til en oppgave.
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        TaskItemStatus status)
    {
        var task = await service.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        task.Status = status;

        return Ok(task);
    }

    /// <summary>
    /// Henter alle oppgaver.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskItem>>> Get()
    {
        var tasks = await service.GetAllAsync();

        return Ok(tasks);
    }


    /// <summary>
    /// Henter én oppgave basert på id.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var task = await service.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }


    /// <summary>
    /// Oppretter en ny oppgave.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Detail = "Title is required",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var createdTask = await service.CreateAsync(task);

        return CreatedAtAction(
            nameof(Get),
            new { id = createdTask.Id },
            createdTask
        );
    }
}