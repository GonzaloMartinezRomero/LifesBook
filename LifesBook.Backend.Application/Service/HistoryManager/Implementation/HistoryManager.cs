using FluentValidation.Results;
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

        public void SaveHistory(DateTime date, HistoryKey historyKey, string history)
        {
            //Validate history key 
            ValidateHistoryKey(historyKey);

            //Check if password is registered
            CheckPasswordRegistered(historyKey);

            //Save             
            string rootHistoryPath = LoadConfigurationValue(AppConfigHelper.ROOT_HISTORY_PATH);
            DirectoryInfo rootDirectory = new DirectoryInfo(rootHistoryPath);

            _historyPersistence.SaveHistory(rootDirectory, date, historyKey, history);
        }

        /// <summary>
        /// Check if password == Stored password
        /// </summary>
        /// <param name="historyKey"></param>
        /// <returns></returns>
        public void CheckPasswordRegistered(HistoryKey historyKey)
        {
            string password = historyKey.Key;

            string passwordHashSaved = LoadConfigurationValue(AppConfigHelper.PASSWORD);
            string hashGenerated = _security.GenerateHash(password);

            if(passwordHashSaved != hashGenerated)
                throw new ApplicationException("Password not registered!");
        }

        private string LoadConfigurationValue(string key)
        {
            string value = _configuration.GetValue<string>(key);

            if (String.IsNullOrEmpty(value))
                throw new ApplicationException($"AppConfiguration: {key} is not defined!");

            return value;
        }

        private static void ValidateHistoryKey(HistoryKey historyKey)
        {
            ValidationResult historyKeyValidation = HistoryKeyValidator.CheckHistoryKey(historyKey);
            if (!historyKeyValidation.IsValid)
                throw new ArgumentException(String.Join(";", historyKeyValidation.Errors.Select(x => x.ErrorMessage)));
        }
    }
}
