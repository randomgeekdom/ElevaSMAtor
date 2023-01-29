using Elevasmator.Services.Interfaces;
using Elevasmator.Services.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services.Tests
{
    public class ElevatorMovementServiceTests
    {
        Mock<ILogger> mockLogger = new Mock<ILogger>();
        Mock<IDelayService> mockDelayService = new Mock<IDelayService>();
        private ElevatorMovementService sut;

        public ElevatorMovementServiceTests()
        {
            this.sut = new ElevatorMovementService(mockLogger.Object, mockDelayService.Object);
        }

        [Fact]
        public async Task WhenMovingToNextFloorAndButtonPressedThenStopElevatorAndResetElevatorButton()
        {
            var sensor = new Sensor
            {
                CurrentOrNextFloor = 2,
                IsGoingUp = true,
                IsMoving = true
            };

            var elevator = new Elevator();
            elevator.InternalButtonPresses.TryUpdate(3, true, false);

            await this.sut.MoveAsync(elevator, sensor, new List<int> { 3 }, CancellationToken.None);

            Assert.False(elevator.InternalButtonPresses[3]);
            this.mockLogger.Verify(x => x.WriteFloorStop(3));
        }

        [Fact]
        public async Task WhenMovingToNextFloorAndButtonNotPressedThenPassFloor()
        {
            var sensor = new Sensor
            {
                CurrentOrNextFloor = 2,
                IsGoingUp = true,
                IsMoving = true
            };

            var elevator = new Elevator();

            await this.sut.MoveAsync(elevator, sensor, new List<int> (), CancellationToken.None);

            this.mockLogger.Verify(x => x.WriteFloorPass(3));
        }
    }
}
