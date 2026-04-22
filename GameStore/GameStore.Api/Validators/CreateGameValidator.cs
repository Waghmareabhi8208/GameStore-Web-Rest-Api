using FluentValidation;
using GameStore.Api.Dtos;

namespace GameStore.Api.Validators;

public class CreateGameValidator : AbstractValidator<CreateGameDto>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Genre is required")
            .MaximumLength(20)
            .WithMessage("Genre must not exceed 20 characters");

        RuleFor(x => x.Price)
            .InclusiveBetween(1, 100)
            .WithMessage("Price must be between 1 and 100");
    }
}