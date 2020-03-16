using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebAPI.Fundamentals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private IWebHostEnvironment _env;

        public EnvironmentController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("===== Get from IWebHostEnvironment =====");
            sb.AppendLine($"IsDevelopment() : {_env.IsDevelopment()}");
            sb.AppendLine($"IsStaging() : {_env.IsStaging()}");
            sb.AppendLine($"IsProduction() : {_env.IsProduction()}");
            sb.AppendLine($"IsEnvironment(\"Development\") : {_env.IsEnvironment("Development")}");

            sb.AppendLine();
            sb.AppendLine("===== Get from Environment.GetEnvironmentVariable() =====");
            var defaultKey = "ASPNETCORE_ENVIRONMENT";
            var customizeKey = "MyEnv";
            sb.AppendLine($"Environment.GetEnvironmentVariable(\"{defaultKey}\") : {Environment.GetEnvironmentVariable(defaultKey)}");
            sb.AppendLine($"Environment.GetEnvironmentVariable(\"{customizeKey}\") : {Environment.GetEnvironmentVariable(customizeKey)}");

            return Ok(sb.ToString());
        }
    }
}