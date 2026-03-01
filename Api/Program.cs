using DTOs.request;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMemDb, MemDb>();
var app = builder.Build();
app.MapPost(
    ApiEndpoints.CREATE,
    (CreateTaskRequest request, IMemDb db) =>
    {
        var task = new ToDoTask
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
        };
        db.AddTask(task);
        return Results.Created(ApiEndpoints.GETById.Replace("{id}", $"{task.Id}"), task);
    }
);
app.MapGet(ApiEndpoints.GET, (IMemDb db) => Results.Ok(db.tasks));
app.MapGet(
    ApiEndpoints.GETById,
    (Guid id, IMemDb db) =>
    {
        var task = db.tasks.FirstOrDefault(a => a.Id == id);
        return Results.Ok(task);
    }
);
app.Run();
