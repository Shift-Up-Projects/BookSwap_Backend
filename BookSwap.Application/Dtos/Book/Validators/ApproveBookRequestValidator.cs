using FluentValidation;
using BookSwap.Application.Dtos.Book.Request;

namespace BookSwap.Application.Dtos.Book.Validators
{
    public class ApproveBookRequestValidator : AbstractValidator<ApproveBookRequest>
    {
        public ApproveBookRequestValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book ID must be greater than 0");
          
            RuleFor(x => x.RejectionReason)
              .NotEmpty().When(x => !x.IsApproved).WithMessage("Rejection reason is required when rejecting a book")
              .Empty().When(x=>x.IsApproved).WithMessage("Rejection reason must be empty when Accept a book")
              .MaximumLength(500).WithMessage("Rejection reason cannot exceed 500 characters");
        }
    }
}