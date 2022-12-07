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
            BuildKeyLengthConfiguration();
        }

        public static ValidationResult CheckHistoryKey(HistoryKey historyKey) => new HistoryKeyValidator().Validate(historyKey);

        private void BuildKeyLengthConfiguration()
        {
            RuleFor(historyKey => historyKey.Key).MinimumLength(MIN_LENGTH_CHARACTER)
                                                 .WithMessage(MinLenghtMessage)
                                                 .MaximumLength(MAX_LENGTH_CHARACTER)
                                                 .WithMessage(MaxLenghtMessage);
        }
    }
}
