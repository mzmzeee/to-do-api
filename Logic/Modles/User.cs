public class User
{
    public required Guid id { get; init; }
    public required string name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    List<ToDoTask> to_do_tasks { get; } = [];
}
