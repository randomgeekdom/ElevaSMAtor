namespace Elevasmator.Services
{
    public interface IElevatorService
    {
        Task OperateAsync(Elevator elevator, Sensor sensor, CancellationToken token);
    }
}