
using BookSwap.Application.Dtos.User.Request;
using FluentValidation;

namespace BookSwap.Application.Dtos.User.Validators
{
    public class AddUserDtoValidator : AbstractValidator<AddUserDto>
    {
        public AddUserDtoValidator()
        {
            RuleFor(dto => dto.UserName)
                .NotNull().WithMessage("Username cannot be null.")
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters and numbers.");

            RuleFor(dto => dto.FirstName)
                .NotNull().WithMessage("First name cannot be null.")
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain letters ");


            RuleFor(dto => dto.LastName)
                .NotNull().WithMessage("Last name cannot be null.")
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last Name  can only contain letters ");


            RuleFor(dto => dto.Email)
                .NotNull().WithMessage("Email cannot be null.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .NotEmpty().WithMessage("Password is required")
                .Length(8, 100).WithMessage("Password must be between 8 and 100 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
                .Matches(@"[-._@+]").WithMessage("Password must contain at least one special character");
        }
    }
}
