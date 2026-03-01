public class ToDoTask
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public bool IsDone { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
