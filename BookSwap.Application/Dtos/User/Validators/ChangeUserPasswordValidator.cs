using BookSwap.Application.Dtos.User.Request;
using FluentValidation;

namespace BookSwap.Application.Dtos.User.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPassword>
    {
        public ChangeUserPasswordValidator()
        {
            RuleFor(dto => dto.UserId)
                .GreaterThan(0).WithMessage("User ID must be a positive integer.");

            RuleFor(dto => dto.CurrentPassword)
                .NotNull().WithMessage("Current password cannot be null.")
                .NotEmpty().WithMessage("Current password is required.")
                .Length(8, 50).WithMessage("Current password must be between 8 and 50 characters.");

            RuleFor(x => x.NewPassword)
                 .NotNull().WithMessage("Password cannot be null.")
                 .NotEmpty().WithMessage("Password is required")
                 .Length(8, 100).WithMessage("Password must be between 8 and 100 characters.")
                 .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                 .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                 .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
                 .Matches(@"[-._@+]").WithMessage("Password must contain at least one special character");
          
            RuleFor(dto => dto.ConfirmPassword)
                .NotNull().WithMessage("Confirm password cannot be null.")
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(dto => dto.NewPassword).WithMessage("Confirm password must match the new password.");
        }
    }
}
