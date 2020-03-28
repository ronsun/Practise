using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI.Fundamentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureHostConfiguration((config) =>
                {
                    config.AddJsonFile("Configuration/config.json", optional: false, reloadOnChange: true);
                    config.AddXmlFile("Configuration/config.xml");
                    config.AddXmlFile("Configuration/notFound.xml", optional: true);
                    config.AddEnvironmentVariables("RON_");
                })
            ;
    }
}
