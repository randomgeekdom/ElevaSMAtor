using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services
{
    public class ElevatorFactory : IElevatorFactory
    {
        private readonly IConfiguration configuration;

        public ElevatorFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Elevator Create()
        {
            if (!int.TryParse(this.configuration["NumberOfFloors"], out var numberOfFloors) || numberOfFloors < 3 || numberOfFloors > 1000)
            {
                numberOfFloors = 25;
            }

            return new Elevator(numberOfFloors);
        }
    }
}
