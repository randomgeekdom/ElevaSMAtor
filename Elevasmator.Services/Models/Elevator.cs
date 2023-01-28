using System.Collections.Concurrent;

namespace Elevasmator.Services.Models
{
    public class Elevator
    {
        private readonly ConcurrentDictionary<int, bool> buttonPresses = new();

        public Elevator() : this(25)
        {
        }

        public Elevator(int numberOfFloors)
        {
            NumberOfFloors = numberOfFloors;
            for (int i = 1; i < numberOfFloors; i++)
            {
                buttonPresses.TryAdd(i, false);
            }
        }

        public int NumberOfFloors { get; }

        public ConcurrentDictionary<int, bool> ButtonPresses => buttonPresses;
    }
}