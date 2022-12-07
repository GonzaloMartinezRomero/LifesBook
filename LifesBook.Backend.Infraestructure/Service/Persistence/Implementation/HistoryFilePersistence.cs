using LifesBook.Backend.Infraestructure.Service.Persistence.Abstract;
using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using LifesBook.Backend.Model;
using Microsoft.Extensions.Configuration;

namespace LifesBook.Backend.Infraestructure.Service.Persistence.Implementation
{
    internal class HistoryFilePersistence : IHistoryPersistence
    {
        private const string DATE_FORMAT = "ddMMyyyy";

        private readonly IHistorySecurity _historySecurity;
        private readonly IConfiguration _configuration;

        public HistoryFilePersistence(IHistorySecurity historySecurity, IConfiguration configuration)
        {
            _historySecurity = historySecurity ?? throw new ArgumentNullException(nameof(IHistorySecurity));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
        }

        public void SaveHistory(DirectoryInfo path, DateTime date, HistoryKey password, string content)
        {
            if (!path.Exists)
                throw new DirectoryNotFoundException(path.FullName);

            //2.-Encrypt content
            string key = password.Key;
            string contentCipher = _historySecurity.Encrypt(key, content);

            string fileName = date.ToString(DATE_FORMAT);
            string writeFilePath = Path.Combine(path.FullName, fileName);   

            //3.-Save replacing file
            File.AppendAllText(writeFilePath, contentCipher, Configuration.FileEncoding);
        }
    }
}
