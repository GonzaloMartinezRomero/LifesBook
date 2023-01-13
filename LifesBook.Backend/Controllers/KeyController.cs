using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using LifesBook.Backend.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LifesBookBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyController : ControllerBase
    {        
        private readonly IHistorySecurity _historySecurity;

        public KeyController(IHistorySecurity historySecurity)
        {
            _historySecurity = historySecurity; 
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(String))]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GenerateKeyEncryption([Required][FromQuery(Name = "Key")] string key)
        {
            try
            {
                string keyEncryption = _historySecurity.GenerateHash(key);  
                return Ok(keyEncryption);
            }            
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}