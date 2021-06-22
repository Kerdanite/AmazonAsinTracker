using System;
using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTrackerCron.ScheduleConfiguration;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AmazonAsinTrackerCron.Jobs
{
    public class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger<CronJobService> _logger;

        public CronJobService(IScheduleConfig<CronJobService> config, ILogger<CronJobService> logger)
        {
            _logger = logger;
            _expression = CronExpression.Parse(config.CronExpression);
            _timeZoneInfo = config.TimeZoneInfo;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)  
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        private  Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob is working.");
            return Task.CompletedTask;
        }
    }
}