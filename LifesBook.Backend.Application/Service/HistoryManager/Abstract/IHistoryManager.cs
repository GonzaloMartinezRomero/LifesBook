using LifesBook.Backend.Model;

namespace LifesBook.Backend.Application.Service.HistoryManager.Abstract
{
    /// <summary>
    /// History manager absctract
    /// </summary>
    public interface IHistoryManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <param name="historyContent"></param>
        /// <returns></returns>
        public History SaveHistory(DateTime date, string key, string historyContent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <param name="historyId"></param>
        /// <returns></returns>
        public History LoadHistory(string key, string historyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<History> LoadAllHistories(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <param name="historyContent"></param>
        /// <returns></returns>
        public History UpdateHistory(string historyId, string key, string historyContent);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyId"></param>
        public void DeleteHistory(string historyId);
    }
}
