using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meteor.Operation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using test.Messages.Test;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly OperationFactory _operationFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, OperationFactory operationFactory)
        {
            _logger = logger;
            _operationFactory = operationFactory;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var msg = _operationFactory.Create<AddTest>();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}