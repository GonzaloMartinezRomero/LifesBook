using LifesBook.Backend.Model;

namespace LifesBook.Backend.Infraestructure.Service.Persistence.Abstract
{
    public interface IHistoryPersistence
    {
        /// <summary>
        /// If exists -> Overwrite
        /// </summary>
        /// <param name="date"></param>
        /// <param name="password"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public History SaveHistory(HistoryKey password, History history, bool overwrite = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="password"></param>
        public History LoadHistory(HistoryKey password, string historyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryContainer"></param>
        /// <param name="historyKey"></param>
        /// <returns></returns>
        public List<History> LoadAllHistories(HistoryKey historyKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyId"></param>
        public void DeleteHistory(string historyId);
    }
}
