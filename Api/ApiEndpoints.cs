using DTOs.request;

public static class ApiEndpoints
{
    public const string Register = "/register";
    public const string Login = "/login";
    public const string CREATE = "/todos";
    public const string GET = "/todos";
    public const string GETById = "/todos/{id}";
    public const string Update = "/todos/{id}";
    public const string Delete = "/todos/{id}";

    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapPost(
            ApiEndpoints.CREATE,
            (CreateTaskRequest request, IMemDb db) =>
            {
                var task = new ToDoTask
                {
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
                if (task is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(task);
            }
        );
        app.MapPut(
            ApiEndpoints.Update,
            (Guid Id, UpdateTaskRequest task, IMemDb db) =>
            {
                var foundtask = db.tasks.FirstOrDefault(x => x.Id == Id);
                if (foundtask is null)
                {
                    return Results.NotFound();
                }
                foundtask.Title = task.Title;
                foundtask.Description = task.Description;

                return Results.Ok(foundtask);
            }
        );
        app.MapDelete(
            ApiEndpoints.Delete,
            (Guid Id, IMemDb db) =>
            {
                var DeletedTask = db.tasks.FirstOrDefault(x => x.Id == Id);
                if (DeletedTask is null)
                {
                    return Results.NotFound();
                }
                db.tasks.Remove(DeletedTask);
                return Results.Ok(DeletedTask);
            }
        );
    }
}
