using System.Collections.Concurrent;

namespace Elevasmator.Services
{
    public class Elevator
    {
        private readonly ConcurrentDictionary<int, bool> buttonPresses = new();

        public Elevator()
        {
            for (int i = 1; i < NumberOfFloors; i++)
            {
                this.buttonPresses.TryAdd(i, false);
            }
        }

        public int NumberOfFloors => 25;

        public bool ArriveAtFloor(int floor)
        {
            return ChangeButtonState(floor, false);
        }

        public bool ShouldStopAtFloor(int floor)
        {
            this.buttonPresses.TryGetValue(floor, out var result);
            return result;
        }

        public IEnumerable<int> GetButtonsPressed()
        {
            return this.buttonPresses.Where(x => x.Value).Select(x => x.Key);
        }

        public bool ChangeButtonState(int floor, bool state)
        {
            return buttonPresses.TryUpdate(floor, state, !state);
        }
    }
}