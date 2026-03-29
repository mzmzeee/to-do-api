using DTOs.request;
using FluentValidation;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {   
        RuleFor(task => task.Title).NotNull().NotEmpty().MaximumLength(30);
        RuleFor(task => task.Description).NotNull().MaximumLength(500);
    }
}
