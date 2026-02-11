namespace WS_CRM.BackgroundJob
{
    public interface IBackgroundTaskQueue
    {
        //ValueTask QueueBackgroundWorkItemAsync(Func<Task> workItem);
        //ValueTask<AIJob> DequeueAsync(CancellationToken token);
        public Task EnqueueAsync(AIJob job);
        public Task<AIJob> DequeueAsync(CancellationToken cancellationToken);
    }
}
