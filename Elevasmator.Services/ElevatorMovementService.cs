using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public class ElevatorMovementService : IElevatorMovementService
    {
        private readonly ILogger logger;

        public ElevatorMovementService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task MoveAsync(Elevator elevator, Sensor sensor, IEnumerable<int> buttonsPressed, CancellationToken token)
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

        public async Task StopAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            sensor.IsMoving = false;
            ArriveAtFloor(elevator, sensor.CurrentOrNextFloor);
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }

        private void ArriveAtFloor(Elevator elevator, int floor)
        {
            elevator.ChangeButtonState(floor, false, ButtonType.Internal);
            elevator.ChangeButtonState(floor, false, ButtonType.ExternalUp);
            elevator.ChangeButtonState(floor, false, ButtonType.ExternalDown);
        }
    }
}
