using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public static class Extensions
    {
        public static void ChangeButtonState(this Elevator elevator, int floor, bool state, ButtonType buttonType)
        {
            elevator.ButtonPresses[buttonType].TryUpdate(floor, state, !state);
        }

        public static void ChangeAllButtonStates(this Elevator elevator, int floor, bool state)
        {
            elevator.ButtonPresses[ButtonType.Internal].TryUpdate(floor, state, !state);
            elevator.ButtonPresses[ButtonType.ExternalDown].TryUpdate(floor, state, !state);
            elevator.ButtonPresses[ButtonType.ExternalUp].TryUpdate(floor, state, !state);
        }

        public static void ChangeDirection(this Sensor sensor)
        {
            sensor.IsGoingUp = !sensor.IsGoingUp;
        }
    }
}
