using Microsoft.Extensions.Configuration;

namespace Elevasmator.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(IConfiguration configuration)
        {
            if (!int.TryParse(configuration["NumberOfFloors"], out var numberOfFloors) || numberOfFloors < 3 || numberOfFloors > 1000)
            {
                numberOfFloors = 25;
            }

            this.NumberOfFloors = numberOfFloors;
        }

        public int NumberOfFloors { get; }
    }
}