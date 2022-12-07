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
        public void SaveHistory(DirectoryInfo path, DateTime date, HistoryKey password, string content);
    }
}
