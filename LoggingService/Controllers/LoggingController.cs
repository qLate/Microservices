using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LoggingController(ILogger<LoggingController> logger) : ControllerBase
    {
        static ConcurrentDictionary<string, string> messages = new();

        // GET: api/<LoggingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return messages.Values;
        }

        // POST api/<LoggingController>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var plainText = await reader.ReadToEndAsync();
            messages.TryAdd(DateTime.Now.ToString(), plainText);
            return Ok();
        }
    }
}