using DTOs.request;
using DTOs.response;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("todos")]
public class ToDosController(IMemDb db) : ControllerBase
{
    private readonly IMemDb _db = db;

    [HttpPost]
    public IActionResult Create([FromBody] TaskRequest request)
    {
        var task = new ToDoTask
        {
            UserId = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
        };

        _db.AddTask(task);

        var returnedtask = new TaskResponse(task.Id, task.Title, task.Description);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, returnedtask);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_db.tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var task = _db.tasks.FirstOrDefault(a => a.Id == id);

        if (task is null)
        {
            return NotFound();
        }
        var returnedtask = new TaskResponse(task.Id, task.Title, task.Description);

        return Ok(returnedtask);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] TaskRequest request)
    {
        var foundTask = _db.tasks.FirstOrDefault(x => x.Id == id);

        if (foundTask is null)
        {
            return NotFound();
        }

        foundTask.Title = request.Title;
        foundTask.Description = request.Description;
        foundTask.IsDone = request.IsDone;

        var returnedtask = new TaskResponse(foundTask.Id, foundTask.Title, foundTask.Description);
        return Ok(returnedtask);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var deletedTask = _db.tasks.FirstOrDefault(x => x.Id == id);

        if (deletedTask is null)
        {
            return NotFound();
        }

        _db.tasks.Remove(deletedTask);

        var returnedtask = new TaskResponse(
            deletedTask.Id,
            deletedTask.Title,
            deletedTask.Description
        );

        return Ok(deletedTask);
    }
}
