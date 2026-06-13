using Blog.Application.Comments.Dtos;
using FluentValidation;

namespace Blog.Application.Comments.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Komentar ne sme biti prazan.")
            .MaximumLength(2000).WithMessage("Komentar ne sme biti duži od 2000 karaktera.");
    }
}
