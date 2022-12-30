namespace LifesBook.Backend.Model
{
    public sealed class History
    {
        public string Id { get; set; } = String.Empty;

        public DateTime Date { get; set; }

        public string Content { get; set; } = String.Empty;

        public History() { }

        public History(DateTime date, string content)
        {   
            Date = date;
            Content = content;
        }       
    }
}
