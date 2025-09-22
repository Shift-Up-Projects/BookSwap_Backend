using FluentValidation;
using BookSwap.Application.Dtos.ExchangeOffer.Request;

namespace BookSwap.Application.Validators
{
    public class RejectExchangeOfferRequestValidator : AbstractValidator<RejectExchangeOfferRequest>
    {
        public RejectExchangeOfferRequestValidator()
        {
            RuleFor(x => x.ExchangeOfferId)
                .GreaterThan(0).WithMessage("Exchange offer ID must be greater than 0");
        }
    }
}