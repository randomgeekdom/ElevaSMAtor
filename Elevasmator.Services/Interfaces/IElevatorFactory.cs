using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public interface IElevatorFactory
    {
        Elevator Create();
    }
}