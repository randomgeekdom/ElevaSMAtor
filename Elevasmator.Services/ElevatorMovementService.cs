using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevasmator.Services.Interfaces;
using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public class ElevatorMovementService : IElevatorMovementService
    {
        private readonly ILogger logger;
        private readonly IDelayService delayService;

        public ElevatorMovementService(ILogger logger, IDelayService delayService)
        {
            this.logger = logger;
            this.delayService = delayService;
        }

        public async Task MoveAsync(Elevator elevator, Sensor sensor, IEnumerable<int> buttonsPressed, CancellationToken token)
        {
            // Start moving toward the next floor
            sensor.IsMoving = true;
            sensor.CurrentOrNextFloor += (sensor.IsGoingUp ? 1 : -1);
            await this.delayService.DelayAsync(3, token);

            if (buttonsPressed.Contains(sensor.CurrentOrNextFloor))
            {
                this.logger.WriteFloorStop(sensor.CurrentOrNextFloor);
                await this.StopAsync(elevator, sensor, token);
            }
            else
            {
                this.logger.WriteFloorPass(sensor.CurrentOrNextFloor);
            }
        }

        public async Task StopAsync(Elevator elevator, Sensor sensor, CancellationToken token)
        {
            sensor.IsMoving = false;
            elevator.ArriveAtFloor(sensor.CurrentOrNextFloor);
            await this.delayService.DelayAsync(1, token);
        }
    }
}
