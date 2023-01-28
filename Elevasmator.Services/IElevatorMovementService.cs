namespace Elevasmator.Services
{
    public interface IElevatorMovementService
    {
        Task MoveAsync(Elevator elevator, Sensor sensor, IEnumerable<int> buttonsPressed, CancellationToken token);

        Task StopAsync(Elevator elevator, Sensor sensor, CancellationToken token);
    }
}