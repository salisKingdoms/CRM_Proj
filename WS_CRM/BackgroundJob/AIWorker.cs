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

                    int retry = 0;
                    bool success = false;
                
                while (retry < 3 && !success)
                {
                    try
                    {
                        await _aiService.AnalyzeAndSaveAsync(
                            workItem.UnitId,
                            workItem.WarrantyNo,
                            workItem.ComplaintText);

                        success = true;
                    }
                    catch 
                    { //retry mechanisme if AI API unstable
                        retry++;
                        await Task.Delay(2000);
                    }
                }
            }
        }
    }
}
