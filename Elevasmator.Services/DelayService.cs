using Elevasmator.Services.Interfaces;

namespace Elevasmator.Services
{
    public class DelayService : IDelayService
    {
        public async Task DelayAsync(int seconds, CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }
}
