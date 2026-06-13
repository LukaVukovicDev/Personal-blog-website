using Blog.Application.Auth.Dtos;
using FluentValidation;

namespace Blog.Application.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Korisničko ime je obavezno.")
            .Length(3, 32).WithMessage("Korisničko ime mora imati između 3 i 32 karaktera.")
            .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Korisničko ime sme sadržati samo slova, brojeve, tačku, crtu i donju crtu.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email je obavezan.")
            .EmailAddress().WithMessage("Email adresa nije validna.")
            .MaximumLength(256).WithMessage("Email ne sme biti duži od 256 karaktera.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Lozinka je obavezna.")
            .MinimumLength(8).WithMessage("Lozinka mora imati najmanje 8 karaktera.")
            .Matches("[A-Z]").WithMessage("Lozinka mora sadržati bar jedno veliko slovo.")
            .Matches("[a-z]").WithMessage("Lozinka mora sadržati bar jedno malo slovo.")
            .Matches("[0-9]").WithMessage("Lozinka mora sadržati bar jednu cifru.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(64).WithMessage("Prikazno ime ne sme biti duže od 64 karaktera.");
    }
}
