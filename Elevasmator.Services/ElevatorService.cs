namespace Elevasmator.Services
{
    public class ElevatorService : IElevatorService
    {
        public async Task OperateAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            var buttonsPressed = elevator.GetButtonsPressed();
            while (buttonsPressed.Any() && !token.IsCancellationRequested)
            {
                // If the elevator hasn't been moving and somebody presses the button for the floor they are on
                if (buttonsPressed.Contains(sensor.CurrentOrNextFloor))
                {
                    await this.StopAsync(elevator, sensor, token);
                }
                else
                {
                    // Start moving toward the next floor
                    sensor.IsMoving = true;
                    sensor.CurrentOrNextFloor += (sensor.IsGoingUp ? 1 : -1);
                    await Task.Delay(TimeSpan.FromSeconds(3), token);

                    // Arrive at the next floor
                    if (buttonsPressed.Contains(sensor.CurrentOrNextFloor))
                    {
                        await this.StopAsync(elevator, sensor, token);
                    }

                    // Determine whether or not to switch directions
                    if (
                        sensor.CurrentOrNextFloor == elevator.NumberOfFloors ||
                        sensor.CurrentOrNextFloor == 1 ||
                        buttonsPressed.Any(x => (sensor.IsGoingUp && x > sensor.CurrentOrNextFloor) || (!sensor.IsGoingUp && x < sensor.CurrentOrNextFloor)))
                    {
                        sensor.IsGoingUp = !sensor.IsGoingUp;
                    }
                }
            }
        }

        private async Task StopAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            sensor.IsMoving = false;
            elevator.ArriveAtFloor(sensor.CurrentOrNextFloor);
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }
}