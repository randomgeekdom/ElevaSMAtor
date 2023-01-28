namespace Elevasmator.Services
{
    public class ElevatorService : IElevatorService
    {
        private readonly ILogger logger;

        public ElevatorService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task OperateAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            var buttonsPressed = elevator.GetButtonsPressed();
            while (buttonsPressed.Any() && !token.IsCancellationRequested)
            {
                // If the elevator hasn't been moving and somebody presses the button for the floor they are on
                if (buttonsPressed.Count() == 1)
                {
                    var button = buttonsPressed.Single();
                    if(button == sensor.CurrentOrNextFloor)
                    {
                        await this.StopAsync(elevator, sensor, token);
                    }
                    else
                    {
                        sensor.IsGoingUp = buttonsPressed.Single() > sensor.CurrentOrNextFloor;
                        await MoveAsync(elevator, sensor, buttonsPressed, token);
                    }
                }
                else
                {
                    await MoveAsync(elevator, sensor, buttonsPressed, token);

                    // Determine whether or not to switch directions
                    if ((buttonsPressed.Any(x => (sensor.IsGoingUp && x > sensor.CurrentOrNextFloor) || (!sensor.IsGoingUp && x < sensor.CurrentOrNextFloor))))
                    {
                        continue;
                    }
                    else if (sensor.CurrentOrNextFloor == elevator.NumberOfFloors || sensor.CurrentOrNextFloor == 1 ||
                        (buttonsPressed.Any(x => (sensor.IsGoingUp && x < sensor.CurrentOrNextFloor) || (!sensor.IsGoingUp && x > sensor.CurrentOrNextFloor))))
                    {
                        sensor.IsGoingUp = !sensor.IsGoingUp;
                    }
                }
            }
        }

        private async Task MoveAsync(Elevator elevator, Sensor sensor, IEnumerable<int> buttonsPressed, CancellationToken token)
        {
            // Start moving toward the next floor
            sensor.IsMoving = true;
            sensor.CurrentOrNextFloor += (sensor.IsGoingUp ? 1 : -1);
            await Task.Delay(TimeSpan.FromSeconds(3), token);

            if (buttonsPressed.Contains(sensor.CurrentOrNextFloor))
            {
                this.logger.Write($"Stopping at floor {sensor.CurrentOrNextFloor}");
                await this.StopAsync(elevator, sensor, token);
            }
            else
            {
                this.logger.Write($"Passing floor {sensor.CurrentOrNextFloor}");
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

        private async Task StopAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            sensor.IsMoving = false;
            elevator.ArriveAtFloor(sensor.CurrentOrNextFloor);
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }
}