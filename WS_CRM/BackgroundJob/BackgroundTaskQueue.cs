using System.Threading.Channels;

namespace WS_CRM.BackgroundJob
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<AIJob> _queue;

        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<AIJob>();
        }

        public async Task EnqueueAsync(AIJob job)
        {
            await _queue.Writer.WriteAsync(job);
        }

        public async Task<AIJob> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }

        //public async ValueTask QueueBackgroundWorkItemAsync(Func<Task> workItem)
        //{
        //    await _queue.Writer.WriteAsync(workItem);
        //}

        //public async ValueTask<Func<Task>> DequeueAsync(CancellationToken token)
        //{
        //    return await _queue.Reader.ReadAsync(token);
        //}
    }
}
