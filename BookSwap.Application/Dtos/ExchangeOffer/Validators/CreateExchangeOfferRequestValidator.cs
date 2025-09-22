using FluentValidation;
using BookSwap.Application.Dtos.ExchangeOffer.Request;

namespace BookSwap.Application.Validators
{
    public class CreateExchangeOfferRequestValidator : AbstractValidator<CreateExchangeOfferRequest>
    {
        public CreateExchangeOfferRequestValidator()
        {
            RuleFor(x => x.RequestedBookId)
                .GreaterThan(0).WithMessage("Requested book ID must be greater than 0");

            RuleFor(x => x.OfferedBookIds)
                .NotEmpty().WithMessage("At least one offered book is required")
                .ForEach(id => id.GreaterThan(0).WithMessage("Each offered book ID must be greater than 0"));
        }
    }
}