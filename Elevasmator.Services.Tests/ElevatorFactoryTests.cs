using Moq;

namespace Elevasmator.Services.Tests
{
    public class ElevatorFactoryTests
    {
        [Fact]
        public void WhenCreatingElevatorReturnElevatorWithNumberOfFloors()
        {
            var mockConfigurationService = new Mock<IConfigurationService>();
            mockConfigurationService.Setup(x => x.NumberOfFloors).Returns(50);
            var sut = new ElevatorFactory(mockConfigurationService.Object);

            var elevator = sut.Create();

            Assert.Equal(50, elevator.NumberOfFloors);

            Assert.Equal(50, elevator.InternalButtonPresses.Count);
            Assert.Equal(50, elevator.ExternalDownButtonPresses.Count);
            Assert.Equal(50, elevator.ExternalUpButtonPresses.Count);

            Assert.True(elevator.InternalButtonPresses.All(x => !x.Value));
            Assert.True(elevator.ExternalDownButtonPresses.All(x => !x.Value));
            Assert.True(elevator.ExternalUpButtonPresses.All(x => !x.Value));

            Assert.Equal(3, elevator.ButtonPresses.Count());
        }
    }
}