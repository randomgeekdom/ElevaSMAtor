using Elevasmator.Services.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services.Tests
{
    public class ElevatorServiceTests
    {
        private Mock<ILogger> mockLogger = new Mock<ILogger>();
        private Mock<IElevatorMovementService> mockElevatorMovementService = new Mock<IElevatorMovementService>();
        private readonly ElevatorService sut;

        public ElevatorServiceTests()
        {
            this.sut = new ElevatorService(mockLogger.Object, mockElevatorMovementService.Object);
        }

        [Theory]
        [InlineData(2, ButtonType.Internal)]
        [InlineData(4, ButtonType.ExternalUp)]
        [InlineData(6, ButtonType.ExternalDown)]
        public void WhenPressingButtonThenButtonStateIsTurnedOn(int level, ButtonType buttonType)
        {
            var elevator = new Elevator();
            this.sut.PressButton(elevator, level, buttonType);

            Assert.True(elevator.ButtonPresses[buttonType][level]);
            mockLogger.Verify(x => x.WriteButtonPress(It.IsAny<int>(), It.IsAny<ButtonType>()));
        }

        [Fact]
        public void WhenPressingButtonForAnInvalidFloorThenNoChange()
        {
            var elevator = new Elevator();
            this.sut.PressButton(elevator, 500, ButtonType.Internal);
            mockLogger.Verify(x => x.WriteButtonPress(It.IsAny<int>(), It.IsAny<ButtonType>()), Times.Never());
        }

        [Fact]
        public async Task WhenOperatingWithoutButtonPressesDoNothing()
        {
            var elevator = new Elevator();
            var sensor = new Sensor();
            await this.sut.OperateAsync(elevator, sensor, CancellationToken.None);

            this.mockElevatorMovementService.Verify(x => x.StopAsync(elevator, sensor, It.IsAny<CancellationToken>()), Times.Never);
            this.mockElevatorMovementService.Verify(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task WhenOperatingWith1ButtonPressedAndIsCurrentFloorThenStopAndOpenDoor()
        {
            var elevator = new Elevator();
            elevator.ExternalDownButtonPresses.TryUpdate(5, true, false);

            var sensor = new Sensor { CurrentOrNextFloor = 5 };
            mockElevatorMovementService.Setup(x => x.StopAsync(elevator, sensor, It.IsAny<CancellationToken>())).Callback(() => {
                elevator.ArriveAtFloor(5);
            });

            await this.sut.OperateAsync(elevator, sensor, CancellationToken.None);


            this.mockElevatorMovementService.Verify(x => x.StopAsync(elevator, sensor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WhenOperatingWith1ButtonPressedAndIsNotCurrentFloorThenMoveTowardFloor()
        {
            var elevator = new Elevator();
            elevator.ExternalDownButtonPresses.TryUpdate(5, true, false);

            var sensor = new Sensor { CurrentOrNextFloor = 3 };
            mockElevatorMovementService.Setup(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>())).Callback(() => {
                elevator.ArriveAtFloor(5);
            });

            await this.sut.OperateAsync(elevator, sensor, CancellationToken.None);


            this.mockElevatorMovementService.Verify(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task WhenOperatingWithMultipleButtonsPressedAndIsNotCurrentFloorThenMoveTowardFloorInCurrentDirection()
        {
            var elevator = new Elevator();
            elevator.ExternalDownButtonPresses.TryUpdate(5, true, false);
            elevator.ExternalDownButtonPresses.TryUpdate(6, true, false);

            var sensor = new Sensor { CurrentOrNextFloor = 3, IsGoingUp = true };
            mockElevatorMovementService.Setup(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>())).Callback(() => {
                elevator.ArriveAtFloor(5);
                elevator.ArriveAtFloor(6);
            });

            await this.sut.OperateAsync(elevator, sensor, CancellationToken.None);


            this.mockElevatorMovementService.Verify(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1, false, true)]
        [InlineData(25, true, false)]
        public async Task WhenOperatingWithMultipleButtonsPressedAndIsOnTopOrBottomFloorThenChangeDirection(int floor, bool startIsGoingUp, bool endIsGoingUp)
        {
            var elevator = new Elevator();
            elevator.ExternalDownButtonPresses.TryUpdate(5, true, false);
            elevator.ExternalDownButtonPresses.TryUpdate(6, true, false);

            var sensor = new Sensor { CurrentOrNextFloor = floor, IsGoingUp = startIsGoingUp };
            mockElevatorMovementService.Setup(x => x.MoveAsync(elevator, sensor, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>())).Callback(() => {
                elevator.ArriveAtFloor(5);
                elevator.ArriveAtFloor(6);
            });

            await this.sut.OperateAsync(elevator, sensor, CancellationToken.None);

            Assert.Equal(endIsGoingUp, sensor.IsGoingUp);
        }
    }
}
