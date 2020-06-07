using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace andymac4182.Reference.Web.Features
{
    [ApiController]
    [Route("envtest")]
    public class Env : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Env(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet]
        public IDictionary GetAllEnv()
        {
            _logger.Debug("Configuration {@Config}", _configuration.AsEnumerable());
            return Environment.GetEnvironmentVariables();
        }
    }
}