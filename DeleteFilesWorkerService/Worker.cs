using DeleteFilesWorkerService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeleteFilesWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IService _service;

        public Worker(ILogger<Worker> logger, IService service)
        {
            _logger = logger;
            _service= service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _service.Execute();
                    await Task.Delay(int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SurveillanceSecond")), stoppingToken);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"{e.Message}{System.Environment.NewLine}{e.StackTrace}");
                }
            }

            if (stoppingToken.IsCancellationRequested)
                _service.Flush();
        }
    }
}
