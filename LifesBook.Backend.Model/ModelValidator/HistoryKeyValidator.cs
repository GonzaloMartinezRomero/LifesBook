using FluentValidation;
using FluentValidation.Results;
using LifesBook.Backend.Model;

namespace LifesBook.Backend.Application.Tool
{
    public sealed class HistoryKeyValidator : AbstractValidator<HistoryKey>
    {
        private const int MAX_LENGTH_CHARACTER = 10;
        private const int MIN_LENGTH_CHARACTER = 5;

        private readonly string MinLenghtMessage = $"Min length is {MIN_LENGTH_CHARACTER}";
        private readonly string MaxLenghtMessage = $"Max length is {MAX_LENGTH_CHARACTER}";

        private HistoryKeyValidator()
        {
            RulesConfiguration();
        }

        public static void ValidateHistoryKey(HistoryKey historyKey)
        {
            var validator = new HistoryKeyValidator().Validate(historyKey);

            if (!validator.IsValid)
                throw new ArgumentException(String.Join(";", validator.Errors.Select(x => x.ErrorMessage)));
        } 

        private void RulesConfiguration()
        {
            RuleFor(historyKey => historyKey.Key).MinimumLength(MIN_LENGTH_CHARACTER)
                                                 .WithMessage(MinLenghtMessage)
                                                 .MaximumLength(MAX_LENGTH_CHARACTER)
                                                 .WithMessage(MaxLenghtMessage);
        }
    }
}
