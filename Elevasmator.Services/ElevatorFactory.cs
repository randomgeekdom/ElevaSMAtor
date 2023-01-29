using Elevasmator.Services.Models;
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
        private readonly IConfigurationService configurationService;

        public ElevatorFactory(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public Elevator Create()
        {
            return new Elevator(this.configurationService.NumberOfFloors);
        }
    }
}
