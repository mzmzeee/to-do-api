using Api.Data;
using DTOs.request;
using DTOs.response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("todos")]
public class ToDosController(
    AppDbContext db,
    IValidator<CreateTaskRequest> createValidator,
    IValidator<UpdateTaskRequest> updateValidator
) : ControllerBase
{
    private readonly AppDbContext _db = db;
    private readonly IValidator<CreateTaskRequest> _createValidator = createValidator;
    private readonly IValidator<UpdateTaskRequest> _updateValidator = updateValidator;

    private async Task<User> GetOrCreateDevUserAsync()
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "dev@local");
        if (user is not null)
        {
            return user;
        }

        user = new User
        {
            Id = Guid.NewGuid(),
            Name = "dev",
            Email = "dev@local",
            PasswordHash = "dev",
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var user = await GetOrCreateDevUserAsync();

        var task = new ToDoTask
        {
            UserId = user.Id,
            Title = request.Title,
            Description = request.Description,
            IsDone = request.IsDone,
        };

        _db.ToDoTasks.Add(task);
        await _db.SaveChangesAsync();

        var returnedtask = new TaskResponse(task.Id, task.Title, task.Description);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, returnedtask);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _db.ToDoTasks
            .AsNoTracking()
            .Select(t => new TaskResponse(t.Id, t.Title, t.Description))
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var task = await _db.ToDoTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (task is null)
        {
            return NotFound();
        }

        var returnedtask = new TaskResponse(task.Id, task.Title, task.Description);

        return Ok(returnedtask);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var foundTask = await _db.ToDoTasks.FirstOrDefaultAsync(x => x.Id == id);

        if (foundTask is null)
        {
            return NotFound();
        }

        foundTask.Title = request.Title;
        foundTask.Description = request.Description;
        foundTask.IsDone = request.IsDone;

        await _db.SaveChangesAsync();

        var returnedtask = new TaskResponse(foundTask.Id, foundTask.Title, foundTask.Description);
        return Ok(returnedtask);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedTask = await _db.ToDoTasks.FirstOrDefaultAsync(x => x.Id == id);

        if (deletedTask is null)
        {
            return NotFound();
        }

        _db.ToDoTasks.Remove(deletedTask);
        await _db.SaveChangesAsync();

        var returnedtask = new TaskResponse(
            deletedTask.Id,
            deletedTask.Title,
            deletedTask.Description
        );

        return Ok(returnedtask);
    }
}
