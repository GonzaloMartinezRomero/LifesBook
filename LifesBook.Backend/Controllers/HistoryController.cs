using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<History>))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ListAllHistories([Required][FromHeader(Name = "Key")] string key)
        {
            try
            {
                List<History> histories = _historyManager.LoadAllHistories(key);
                return Ok(histories);
            }
            catch (KeyNotFoundException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (ArgumentException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{historyId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(History))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LoadHistory([Required]string historyId, 
                                         [Required][FromHeader(Name = "Key")] string key)
        {
            try
            {   
                History history = _historyManager.LoadHistory(key, historyId);

                return Ok(history);
            }
            catch (KeyNotFoundException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Create a new history
        /// </summary>
        /// <param name="key"></param>
        /// <param name="date"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(History))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult WriteHistory([Required][FromHeader(Name = "Key")] string key, 
                                          [Required][FromQuery] DateTime date,
                                          [FromBody] string historyContent)
        {
            try
            {
                History history = _historyManager.SaveHistory(date, key, historyContent);
                return StatusCode(StatusCodes.Status201Created, history);
            }            
            catch(KeyNotFoundException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{historyId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(History))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateHistory([Required][FromHeader(Name = "Key")] string key,
                                          [Required] string historyId,
                                          [FromBody] string historyContent)
        {
            try
            {
                History history = _historyManager.UpdateHistory(historyId, key, historyContent);
                return StatusCode(StatusCodes.Status201Created, history);
            }
            catch (KeyNotFoundException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{historyId}")]
        public IActionResult DeleteHistory(string historyId)
        {
            try
            {
                _historyManager.DeleteHistory(historyId);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (KeyNotFoundException errorKey)
            {
                return StatusCode(StatusCodes.Status409Conflict, errorKey.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}