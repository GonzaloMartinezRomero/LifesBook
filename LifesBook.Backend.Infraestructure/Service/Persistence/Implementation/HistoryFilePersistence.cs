using LifesBook.Backend.Infraestructure.Service.Persistence.Abstract;
using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using LifesBook.Backend.Model;
using Microsoft.Extensions.Configuration;
using System.Globalization;

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

        public History LoadHistory(HistoryKey password, string historyId)
        {
            DirectoryInfo directoryContainer = LoadDirectoryContainer();

            string fileIdentificator = historyId ?? String.Empty;
            string fullPathContainer = Path.Combine(directoryContainer.FullName, fileIdentificator);

            string content = File.ReadAllText(fullPathContainer, ConfigurationValue.FileEncoding);

            string historyText = _historySecurity.Decrypt(password.Key, content);

            History historySaved = BuildHistory(fileIdentificator, historyText);

            return historySaved;
        }

        public History SaveHistory(HistoryKey password, History history, bool overwrite = false)
        {
            DirectoryInfo directoryContainer = LoadDirectoryContainer();

            History historySaved = history;

            //Encrypt content with the given key
            string key = password.Key;
            string content = historySaved.Content ?? String.Empty;
            string contentCipher = _historySecurity.Encrypt(key, content);

            //Set identifier
            string fileIdentificator = BuildFileIdentificator(historySaved.Date);
            historySaved.Id = fileIdentificator;
            
            string writeFilePath = Path.Combine(directoryContainer.FullName, fileIdentificator);

            if (File.Exists(writeFilePath))
            {
                if(overwrite)
                    File.Delete(writeFilePath);
                else
                    throw new InvalidOperationException($"File {writeFilePath} already exists");
            }

            //Save file            
            File.AppendAllText(writeFilePath, contentCipher, ConfigurationValue.FileEncoding);

            return historySaved;
        }

        public List<History> LoadAllHistories(HistoryKey historyKey)
        {
            DirectoryInfo directoryContainer = LoadDirectoryContainer();

            string key = historyKey.Key;

            List<History> histories = new List<History>();

            FileInfo[] files = directoryContainer.GetFiles();

            foreach (FileInfo file in files)
            {
                string path = file.FullName;

                string content = File.ReadAllText(path, ConfigurationValue.FileEncoding);

                string contentDecrypted = _historySecurity.Decrypt(key, content);

                History history = BuildHistory(file.Name, contentDecrypted);

                histories.Add(history);
            }

            return histories;
        }

        public void DeleteHistory(string historyId)
        {
            DirectoryInfo directoryContainer = LoadDirectoryContainer();

            string fileIdentificator = historyId ?? String.Empty;
            string fullPathContainer = Path.Combine(directoryContainer.FullName, fileIdentificator);

            File.Delete(fullPathContainer);
        }

        private static string BuildFileIdentificator(DateTime fileDate) => fileDate.ToString(DATE_FORMAT);

        private static History BuildHistory(string historyId, string historyText)
        {
            DateTime dt = DateTime.ParseExact(historyId, DATE_FORMAT, CultureInfo.InvariantCulture);

            History history = new History()
            {
                Id = historyId,
                Date = dt,
                Content = historyText
            };

            return history;
        }

        private DirectoryInfo LoadDirectoryContainer()
        {
            string value = _configuration.GetValue<string>(ConfigurationValue.ROOT_HISTORY_PATH) ?? String.Empty;

            if (String.IsNullOrEmpty(value))
                throw new ApplicationException($"AppConfiguration: {ConfigurationValue.ROOT_HISTORY_PATH} is not defined!");
                       
            DirectoryInfo rootDirectoryContainer = new DirectoryInfo(value);

            if (!rootDirectoryContainer.Exists)
                throw new DirectoryNotFoundException(rootDirectoryContainer.FullName);

            return rootDirectoryContainer;
        }
    }
}
