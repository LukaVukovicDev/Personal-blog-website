using Blog.Application.Categories.Dtos;
using FluentValidation;

namespace Blog.Application.Categories.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv kategorije je obavezan.")
            .MaximumLength(100).WithMessage("Naziv kategorije ne sme biti duži od 100 karaktera.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Opis ne sme biti duži od 500 karaktera.");
    }
}
