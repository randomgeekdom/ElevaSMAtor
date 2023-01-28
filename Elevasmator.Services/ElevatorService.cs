namespace Elevasmator.Services
{
    public class ElevatorService
    {
        public Task? ElevatorOperation { get; set; }

        public void StartupElevator(Elevator elevator, CancellationToken token)
        {
            this.ElevatorOperation = Task.Run(()=>OperateElevator(elevator, token), token);
        }

        private void OperateElevator(Elevator elevator, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {

            }
        }
    }
}