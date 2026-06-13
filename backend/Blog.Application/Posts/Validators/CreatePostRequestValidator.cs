using Blog.Application.Common.Validators;
using Blog.Application.Posts.Dtos;
using FluentValidation;

namespace Blog.Application.Posts.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Naslov je obavezan.")
            .MaximumLength(200).WithMessage("Naslov ne sme biti duži od 200 karaktera.");

        RuleFor(x => x.Excerpt)
            .NotEmpty().WithMessage("Kratak opis je obavezan.")
            .MaximumLength(500).WithMessage("Kratak opis ne sme biti duži od 500 karaktera.");

        RuleFor(x => x.ContentHtml)
            .NotEmpty().WithMessage("Sadržaj posta je obavezan.");

        RuleFor(x => x.CoverImageUrl)
            .MaximumLength(2048).WithMessage("URL naslovne slike ne sme biti duži od 2048 karaktera.")
            .Must(UrlValidator.BeHttpUrlOrEmpty).WithMessage("URL naslovne slike mora biti http(s) link.");

        RuleForEach(x => x.TagNames)
            .NotEmpty().WithMessage("Naziv taga ne sme biti prazan.")
            .MaximumLength(50).WithMessage("Naziv taga ne sme biti duži od 50 karaktera.");
    }
}
