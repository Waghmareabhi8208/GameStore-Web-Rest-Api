using FluentValidation;
using GameStore.Api.Dtos;

namespace GameStore.Api.Validators;

public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
{
    public UpdateGameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters");

        RuleFor(x => x.GenreId)
            .GreaterThan(0)
            .WithMessage("GenreId is required");
        

        RuleFor(x => x.Price)
            .InclusiveBetween(1, 100)
            .WithMessage("Price must be between 1 and 100");
    }
}