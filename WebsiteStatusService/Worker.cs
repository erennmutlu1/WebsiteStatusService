using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebsiteStatusService
{

    // Worker Service is a service that runs in the background forever
    // In windows - Services that run in background
    // In Linux they are Deamon
    // today we are creating a service to monitor our website to monitor that it is up
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The Service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.github.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The Web site is up :) Status Code {StatusCode}", result.StatusCode);
                }
                else
                {
                  
                    _logger.LogError("Booooooooo!!!The Web site is broken:( Status code {StatusCode}", result.StatusCode);
                };


                Console.WriteLine(result.StatusCode);
                await Task.Delay(3*1000, stoppingToken);
            }
        }
    }
}
