using System.Collections.Generic;
using System.Linq;

namespace Elevasmator.Services
{
    public class Sensor
    {
        public bool IsGoingUp { get; set; } = true;
        public int CurrentOrNextFloor { get; set; } = 1;
        public bool IsMoving { get; set; }
        public bool ExceedsWeightLimit { get; set; }
    }
}