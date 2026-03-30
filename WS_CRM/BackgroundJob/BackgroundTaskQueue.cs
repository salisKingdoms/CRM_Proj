using System.Threading.Channels;

namespace WS_CRM.BackgroundJob
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<AIJob> _queue;

        public BackgroundTaskQueue()
        {
             _queue = Channel.CreateBounded<AIJob>(new BoundedChannelOptions(500)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
        }

        public async Task EnqueueAsync(AIJob job)
        {
            await _queue.Writer.WriteAsync(job);
        }

        public async Task<AIJob> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }

    }
}
