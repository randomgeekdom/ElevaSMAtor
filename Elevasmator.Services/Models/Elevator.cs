using System.Collections.Concurrent;

namespace Elevasmator.Services.Models
{
    public class Elevator
    {
        private readonly ConcurrentDictionary<ButtonType, ConcurrentDictionary<int, bool>> buttonPresses = new();

        public Elevator() : this(25)
        {
        }

        public Elevator(int numberOfFloors)
        {
            NumberOfFloors = numberOfFloors;

            buttonPresses.TryAdd(ButtonType.Internal, new ConcurrentDictionary<int, bool>());
            buttonPresses.TryAdd(ButtonType.ExternalDown, new ConcurrentDictionary<int, bool>());
            buttonPresses.TryAdd(ButtonType.ExternalUp, new ConcurrentDictionary<int, bool>());

            for (int i = 1; i <= numberOfFloors; i++)
            {
                buttonPresses[ButtonType.Internal].TryAdd(i, false);
                buttonPresses[ButtonType.ExternalDown].TryAdd(i, false);
                buttonPresses[ButtonType.ExternalUp].TryAdd(i, false);
            }
        }

        public int NumberOfFloors { get; }

        public ConcurrentDictionary<int, bool> InternalButtonPresses => buttonPresses[ButtonType.Internal];
        public ConcurrentDictionary<int, bool> ExternalUpButtonPresses => buttonPresses[ButtonType.ExternalUp];
        public ConcurrentDictionary<int, bool> ExternalDownButtonPresses => buttonPresses[ButtonType.ExternalDown];

        public ConcurrentDictionary<ButtonType, ConcurrentDictionary<int, bool>> ButtonPresses => buttonPresses;
    }
}