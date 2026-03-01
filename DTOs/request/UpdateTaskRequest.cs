namespace DTOs.request;

public record UpdateTaskRequest(Guid Id, string Title, string Description);
