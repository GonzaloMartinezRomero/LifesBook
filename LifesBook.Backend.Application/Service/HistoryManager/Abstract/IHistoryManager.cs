using LifesBook.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifesBook.Backend.Application.Service.HistoryManager.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHistoryManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">History Date</param>
        /// <param name="history">Hisory content</param>
        /// <param name="password"></param>
        public void SaveHistory(DateTime date, HistoryKey historyKey, string history);
    }
}
