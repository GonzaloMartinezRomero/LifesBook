using LifesBook.Backend.Application.Tool;

namespace LifesBook.Backend.Model
{
    public class HistoryKey
    {
        public string Key { get; private set; } = String.Empty;

        private HistoryKey() { }

        public HistoryKey(string key)
        {
            Key = key;

            HistoryKeyValidator.ValidateHistoryKey(this);
        }
    }
}