using DTOs.request;
using FluentValidation;
public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(task => task.Title).NotNull().NotEmpty().MaximumLength(30);
        RuleFor(task => task.Description).NotNull().MaximumLength(500);
    }
}
