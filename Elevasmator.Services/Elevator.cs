using System.Collections.Concurrent;

namespace Elevasmator.Services
{
    public class Elevator
    {
        const int NumberOfFloors = 25;
        private Sensor Sensor { get; } = new Sensor();
        private readonly ConcurrentDictionary<int, bool> buttonPresses = new ();

        public Elevator()
        {
            for (int i = 0; i < NumberOfFloors; i++)
            {
                this.buttonPresses.TryAdd(1, false);
            }
        }

        public void PressButton(int floor)
        {
            buttonPresses[floor] = true;
        }

        public void ArriveAtFloor(int floor)
        {
            buttonPresses[floor] = false;
        }
    }
}