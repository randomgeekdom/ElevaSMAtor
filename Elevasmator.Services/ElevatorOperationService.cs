namespace Elevasmator.Services
{
    public class ElevatorOperationService
    {
        private readonly IElevatorService elevatorService;

        public ElevatorOperationService(IElevatorService elevatorService)
        {
            this.elevatorService = elevatorService;
        }

        public async Task StartupElevator(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await this.elevatorService.OperateAsync(elevator, sensor, token);
            }
        }
    }
}