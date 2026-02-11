namespace WS_CRM.BackgroundJob
{
    public class AIWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;
        private readonly GroqAIService _aiService;

        public AIWorker(IBackgroundTaskQueue queue, GroqAIService aiService)
        {
            _queue = queue;
            _aiService = aiService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _queue.DequeueAsync(stoppingToken);

                try
                {
                    await _aiService.AnalyzeAndSaveAsync(
                        workItem.UnitId,
                        workItem.WarrantyNo,
                        workItem.ComplaintText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AI worker error: {ex.Message}");
                }
            }
        }
    }
}
