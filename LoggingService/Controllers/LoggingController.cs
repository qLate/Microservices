﻿using System.Collections.Concurrent;
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
            logger.LogInformation("Received GET request");

            return messages.Values;
        }

        // POST api/<LoggingController>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var plainText = await reader.ReadToEndAsync();

            logger.LogInformation("Received POST request, value: {0}", plainText);

            messages.TryAdd(DateTime.Now.ToString(), plainText);
            return Ok();
        }
    }
}