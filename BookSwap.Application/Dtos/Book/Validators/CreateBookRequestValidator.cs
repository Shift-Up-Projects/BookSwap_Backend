using FluentValidation;
using BookSwap.Core.Enums;
using BookSwap.Application.Dtos.Book.Request;

namespace BookSwap.Application.Dtos.Book.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required")
                .MaximumLength(100).WithMessage("Author cannot exceed 100 characters");

            RuleFor(x => x.ISBN)
                .Matches(@"^(?:\d{10}|\d{13})$").When(x => !string.IsNullOrEmpty(x.ISBN))
                .WithMessage("ISBN must be 10 or 13 digits");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
          
            RuleFor(dto => dto.Image)
               .Must(image => image is null ? false : image.Length <= 5 * 1024 * 1024).WithMessage("Image size cannot exceed 5 MB.")
               .Must(image => image is null ? false : new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower()))
               .WithMessage("Invalid image format. Only PNG, JPG, and JPEG are allowed");

            RuleFor(x => x.Condition)
                   .IsInEnum().WithMessage("Condition must be New, LikeNew, Good, Fair ");

            RuleFor(x => x.BookStatus)
                     .Must(status => status == BookStatus.Available || status == BookStatus.Personal)
                     .WithMessage("Status must be either Available or Personal");
           
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category Id must be greater than 0");
        }
    }
}