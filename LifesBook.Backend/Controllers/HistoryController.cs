using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LifesBookBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly IHistoryManager _historyManager;

        public HistoryController(ILogger<HistoryController> logger, IHistoryManager historyManager)
        {
            _logger = logger;
            _historyManager = historyManager;   
        }

        [HttpGet()]
        public void ListAllHistories()
        {

        }

        [HttpGet("Open")]
        public string OpenHistory([Required] [FromQuery] DateTime date)
        {
            return "asdfasdfad";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="date"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult WriteHistory([Required][FromHeader(Name ="Key")] string key, 
                                          [Required][FromQuery] DateTime date,
                                          [FromBody] string history)
        {
            try
            {
                HistoryKey historyKey = new HistoryKey(key);

                _historyManager.SaveHistory(date, historyKey, history);
                return Ok();
            }            
            catch(ArgumentException argExcept)
            {
                return UnprocessableEntity(argExcept.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete()]
        public void DeleteHistory()
        {

        }
    }
}