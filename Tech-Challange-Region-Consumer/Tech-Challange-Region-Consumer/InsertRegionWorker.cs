namespace Tech_Challange_Region_Consumer
{
    public class InsertRegionWorker : BackgroundService
    {
        private readonly ILogger<InsertRegionWorker> _logger;
        

        public InsertRegionWorker(ILogger<InsertRegionWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
