using System;
using System.Threading;
using System.Threading.Tasks;

namespace Meridian.CatalogManagement.Core.Interfaces.BackgroundTasks
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken);
    }
}
