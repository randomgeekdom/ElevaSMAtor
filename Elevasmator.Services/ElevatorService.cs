namespace Elevasmator.Services
{
    public class ElevatorService : IElevatorService
    {
        private readonly ILogger logger;
        private readonly IElevatorMovementService elevatorMovementService;

        public ElevatorService(ILogger logger, IElevatorMovementService elevatorMovementService)
        {
            this.logger = logger;
            this.elevatorMovementService = elevatorMovementService;
        }

        public async Task OperateAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            var buttonsPressed = GetButtonsPressed(elevator);
            while (buttonsPressed.Any() && !token.IsCancellationRequested)
            {
                // If the elevator hasn't been moving and somebody presses the button for the floor they are on
                if (buttonsPressed.Count() == 1)
                {
                    var button = buttonsPressed.Single();
                    if (button == sensor.CurrentOrNextFloor)
                    {
                        await this.elevatorMovementService.StopAsync(elevator, sensor, token);
                    }
                    else
                    {
                        sensor.IsGoingUp = buttonsPressed.Single() > sensor.CurrentOrNextFloor;
                        await this.elevatorMovementService.MoveAsync(elevator, sensor, buttonsPressed, token);
                    }
                }
                else
                {
                    await this.elevatorMovementService.MoveAsync(elevator, sensor, buttonsPressed, token);

                    // if there are any floors with buttons pressed in the same direction, then keep going
                    if ((buttonsPressed.Any(x => (sensor.IsGoingUp && x > sensor.CurrentOrNextFloor) || (!sensor.IsGoingUp && x < sensor.CurrentOrNextFloor))))
                    {
                        continue;
                    }
                    // if top floor, bottom floor, or there are floors in the opposite direction with buttons pressed, change direction
                    else if (sensor.CurrentOrNextFloor == elevator.NumberOfFloors || sensor.CurrentOrNextFloor == 1 ||
                        (buttonsPressed.Any(x => (sensor.IsGoingUp && x < sensor.CurrentOrNextFloor) || (!sensor.IsGoingUp && x > sensor.CurrentOrNextFloor))))
                    {
                        sensor.ChangeDirection();
                    }
                }
            }
        }

        public bool PressButton(Elevator elevator, int floor)
        {
            if (floor >= 1 && floor <= elevator.NumberOfFloors)
            {
                var result = elevator.ChangeButtonState(floor, true);
                this.logger.Write($"Button pressed for floor {floor}");
                return result;
            }

            return false;
        }

        private IEnumerable<int> GetButtonsPressed(Elevator elevator)
        {
            return elevator.ButtonPresses.Where(x => x.Value).Select(x => x.Key);
        }

    }
}