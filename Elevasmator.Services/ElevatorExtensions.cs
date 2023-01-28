using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services
{
    public static class Extensions
    {
        public static bool ChangeButtonState(this Elevator elevator, int floor, bool state)
        {
            return elevator.ButtonPresses.TryUpdate(floor, state, !state);
        }

        public static void ChangeDirection(this Sensor sensor)
        {
            sensor.IsGoingUp = !sensor.IsGoingUp;
        }
    }
}
