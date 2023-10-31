using FluentValidation;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Validators
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(item => item.Name).NotEmpty();
            RuleFor(item => item.Name.GetType()).Equals(typeof(string));

            RuleFor(item => item.Price).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(item => item.Price.GetType()).Equals(typeof(int));
        }
    }
}
