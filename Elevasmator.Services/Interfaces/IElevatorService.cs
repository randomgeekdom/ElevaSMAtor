using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public interface IElevatorService
    {
        Task OperateAsync(Elevator elevator, Sensor sensor, CancellationToken token);
        void PressButton(Elevator elevator, int floor, ButtonType buttonType);
    }
}