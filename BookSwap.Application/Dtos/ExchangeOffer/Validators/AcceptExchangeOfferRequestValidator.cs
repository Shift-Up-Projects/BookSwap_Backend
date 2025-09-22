using FluentValidation;
using BookSwap.Application.Dtos.ExchangeOffer.Request;

namespace BookSwap.Application.Validators
{
    public class AcceptExchangeOfferRequestValidator : AbstractValidator<AcceptExchangeOfferRequest>
    {
        public AcceptExchangeOfferRequestValidator()
        {
            RuleFor(x => x.ExchangeOfferId)
                .GreaterThan(0).WithMessage("Exchange offer ID must be greater than 0");

            RuleFor(x => x.SelectedBookId)
                .GreaterThan(0).WithMessage("Selected book ID must be greater than 0");
        }
    }
}