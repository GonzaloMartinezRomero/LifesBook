using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Application.Tool;
using LifesBook.Backend.Infraestructure.Service.Persistence.Abstract;
using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using LifesBook.Backend.Model;
using Microsoft.Extensions.Configuration;

namespace LifesBook.Backend.Application.Service.HistoryManager.Implementation
{
    internal sealed class HistoryManager : IHistoryManager
    {
        private readonly IHistorySecurity _security;
        private readonly IHistoryPersistence _historyPersistence;
        private readonly IConfiguration _configuration;

        public HistoryManager(IHistorySecurity security, IHistoryPersistence historyDataBase, IConfiguration configuration)
        {
            _security = security ?? throw new ArgumentNullException(nameof(IHistorySecurity));
            _historyPersistence = historyDataBase ?? throw new ArgumentNullException(nameof(IHistoryPersistence));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
        }

        /// <inheritdoc/>
        public History SaveHistory(DateTime date, string key, string historyContent)
        {
            HistoryKey historyKey = new HistoryKey(key);

            //Check if password is registered
            CheckKeyRegistered(historyKey);

            //Save             
            History history = new History(date, historyContent);

            History historySaved = _historyPersistence.SaveHistory(historyKey, history);

            return historySaved;
        }

        public History LoadHistory(string key, string historyId)
        {
            HistoryKey historyKey = new HistoryKey(key);

            CheckKeyRegistered(historyKey);

            History history = _historyPersistence.LoadHistory(historyKey, historyId);

            return history;
        }

        public List<History> LoadAllHistories(string key)
        {
            HistoryKey historyKey = new HistoryKey(key);

            CheckKeyRegistered(historyKey);

            List<History> historyList = _historyPersistence.LoadAllHistories(historyKey)
                                                           .OrderBy(x=>x.Date)
                                                           .ToList();

            return historyList;
        }

        public History UpdateHistory(string historyId, string key, string historyContent)
        {
            HistoryKey historyKey = new HistoryKey(key);

            CheckKeyRegistered(historyKey);

            //Update content
            History history = _historyPersistence.LoadHistory(historyKey, historyId);
            history.Content = historyContent;

            history = _historyPersistence.SaveHistory(historyKey, history, overwrite: true);

            return history;
        }

        public void DeleteHistory(string historyId)
        {           
            _historyPersistence.DeleteHistory(historyId);
        }

        private void CheckKeyRegistered(HistoryKey historyKey)
        {
            string key = historyKey.Key;

            string passwordHashSaved = LoadConfigurationValue(AppConfigHelper.PASSWORD);
            string hashGenerated = _security.GenerateHash(key);

            if (passwordHashSaved != hashGenerated)
                throw new KeyNotFoundException("Key is not registered");
        }

        private string LoadConfigurationValue(string key)
        {
            string value = _configuration.GetValue<string>(key) ?? String.Empty;

            if (String.IsNullOrEmpty(value))
                throw new ApplicationException($"AppConfiguration: {key} is not defined!");

            return value;
        }      
    }
}
