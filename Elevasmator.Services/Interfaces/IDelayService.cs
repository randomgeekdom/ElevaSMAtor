using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services.Interfaces
{
    public interface IDelayService
    {
        Task DelayAsync(int seconds, CancellationToken token);
    }
}
