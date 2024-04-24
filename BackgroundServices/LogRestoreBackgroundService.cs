namespace WebAPISample.BackgroundServices
{
    public class LogRestoreBackgroundService : BackgroundService
    {
        private readonly ILogger<LogRestoreBackgroundService> _logger;
        private readonly TimeSpan _timeSpan;
        public LogRestoreBackgroundService(ILogger<LogRestoreBackgroundService> logger)
        {
            _logger = logger;
            _timeSpan = TimeSpan.FromMinutes(1);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(LogRestoreBackgroundService)} starting!");

            while (!stoppingToken.IsCancellationRequested)
            {
                //確認Log資料並回補

                _logger.LogInformation($"{nameof(LogRestoreBackgroundService)} done!");
                await Task.Delay(_timeSpan, stoppingToken);
            }
        }
    }
}
