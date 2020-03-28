using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Fundamentals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;

        public ConfigurationController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("===== All loaded configuration settings =====");
            var allConfigs = _configuration.AsEnumerable();
            foreach (var config in allConfigs)
            {
                sb.AppendLine($"{config.Key} : {config.Value}");
            }

            sb.AppendLine();
            sb.AppendLine("===== Get configuration by multiple ways =====");
            var fromSection = _configuration.GetSection("section0:subsection0:string0").Value;
            var fromGetValue = _configuration.GetValue<string>("section0:subsection0:string0");
            var fromIndexer = _configuration["section0:subsection0:string0"];
            var fromMix1 = _configuration.GetSection("section0:subsection0").GetValue<string>("string0");
            var fromMix2 = _configuration.GetSection("section0").GetValue<string>("subsection0:string0");
            var fromMix3 = _configuration.GetSection("section0")["subsection0:string0"];
            sb.AppendLine($"{nameof(fromSection)}: {fromSection}");
            sb.AppendLine($"{nameof(fromGetValue)}: {fromGetValue}");
            sb.AppendLine($"{nameof(fromIndexer)}: {fromIndexer}");
            sb.AppendLine($"{nameof(fromMix1)}: {fromMix1}");
            sb.AppendLine($"{nameof(fromMix2)}: {fromMix2}");
            sb.AppendLine($"{nameof(fromMix3)}: {fromMix3}");

            sb.AppendLine();
            sb.AppendLine("===== Ways will return value in string by default =====");
            var numberFromIndexer = _configuration["section0:number0"];
            var booleanFromIndexer = _configuration["section0:boolean0"];
            var numberFromSection = _configuration.GetSection("section0:number0").Value;
            var booleanFromSection = _configuration.GetSection("section0:boolean0").Value;
            sb.AppendLine($"{nameof(numberFromIndexer)}: {numberFromIndexer} with type {numberFromIndexer.GetType()}");
            sb.AppendLine($"{nameof(booleanFromIndexer)}: {booleanFromIndexer} with type {booleanFromIndexer.GetType()}");
            sb.AppendLine($"{nameof(numberFromSection)}: {numberFromSection} with type {numberFromSection.GetType()}");
            sb.AppendLine($"{nameof(booleanFromSection)}: {booleanFromSection} with type {booleanFromSection.GetType()}");

            sb.AppendLine();
            sb.AppendLine("===== Get items in array =====");
            var array0 = _configuration.GetValue<string>("section0:array0:0");
            var array1 = _configuration.GetValue<string>("section0:array0:1");
            sb.AppendLine($"{nameof(array0)}: {array0}");
            sb.AppendLine($"{nameof(array1)}: {array1}");

            sb.AppendLine();
            sb.AppendLine("===== Get from other config files =====");
            var fromAnotherXML = _configuration.GetValue<string>("xmlElement");
            var fromAnotherJson = _configuration.GetValue<string>("customizeJson:key");
            sb.AppendLine($"{nameof(fromAnotherXML)}: {fromAnotherXML}");
            sb.AppendLine($"{nameof(fromAnotherJson)}: {fromAnotherJson}");

            sb.AppendLine();
            sb.AppendLine("===== Get children =====");
            var children = _configuration.GetSection("section0").GetChildren();
            foreach (var child in children)
            {
                sb.AppendLine($"{nameof(child)} key under section0: {child.Key}");
            }

            sb.AppendLine();
            sb.AppendLine("===== Case insensitive =====");
            var caseInsensitive1 = _configuration.GetSection("Section0:String0").Value;
            var caseInsensitive2 = _configuration.GetValue<string>("Section0:String0");
            sb.AppendLine($"GetSection(\"Section0:String0\"): {caseInsensitive1}");
            sb.AppendLine($"GetValue<string>(\"Section0:String0\"): {caseInsensitive2}");

            sb.AppendLine();
            sb.AppendLine("===== Get and parse to model (case insensitive, too) =====");
            // Note: Property Name must match the name in config (case insensitive), 
            //       alias seems not supported.
            // var doNotWork = _configuration.GetValue<Section>("section0");
            var section0 = _configuration.GetSection("section0").Get<Section>();
            sb.AppendLine($"Type is {section0.GetType()}");
            sb.AppendLine($"{nameof(Section.String0)}: {section0.String0}");
            sb.AppendLine($"{nameof(Section.Number0)}: {section0.Number0}");
            sb.AppendLine($"{nameof(Section.Boolean0)}: {section0.Boolean0}");

            sb.AppendLine($"{nameof(Section.Array0)}[0]: {section0.Array0[0]}");
            sb.AppendLine($"{nameof(Section.Array0)}[1]: {section0.Array0[1]}");

            sb.AppendLine();
            sb.AppendLine("===== Use last one if key duplicate =====");
            var stringDev = _configuration.GetValue<string>("replacedString");
            sb.AppendLine($"Current environment: {_env.EnvironmentName}");
            sb.AppendLine($"{nameof(stringDev)}: {stringDev}");

            sb.AppendLine();
            sb.AppendLine("===== Duplicate key for array replaced by index order =====");
            var arrayDev = _configuration.GetSection("replacedArray").Get<string[]>();
            sb.AppendLine($"Current environment: {_env.EnvironmentName}");
            sb.AppendLine($"{nameof(arrayDev)}[0]: {arrayDev[0]}");
            sb.AppendLine($"{nameof(arrayDev)}[1]: {arrayDev[1]}");
            
            return Ok(sb.ToString());
        }

        public class Section
        {
            public SubSection Subsection0 { get; set; }

            public string String0 { get; set; }

            public int Number0 { get; set; }

            public bool Boolean0 { get; set; }

            public List<string> Array0 { get; set; }
        }

        public class SubSection
        {
            public string String0 { get; set; }
        }
    }
}