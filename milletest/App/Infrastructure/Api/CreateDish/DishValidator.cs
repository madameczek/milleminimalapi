using FastEndpoints;

namespace milletest.App.Infrastructure.Api.CreateDish;

public class DishValidator : Validator<RequestDto>
{
    public DishValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters");
    }
}