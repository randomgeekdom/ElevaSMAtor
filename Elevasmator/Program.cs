using Elevasmator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevasmator
{
    internal class Program
    {
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    (_, services) => services
                        .AddTransient<ElevatorOperationService>()
                        .AddSingleton<IElevatorService, ElevatorService>());
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var operationService = host.Services.GetRequiredService<ElevatorOperationService>();
            var elevatorService = host.Services.GetRequiredService<IElevatorService>();

            var tokenSource = new CancellationTokenSource();

            var elevator = new Elevator();
            var sensor = new Sensor();

            Task.Factory.StartNew(() => operationService.StartupElevator(elevator, sensor, tokenSource.Token));

            var continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("INPUT ==> ");
                var input = Console.ReadLine();

                input = input?.ToLower().Trim();

                if (input == "q")
                {
                    tokenSource.Cancel();
                    continueProgram = false;
                }

                if(input != null)
                {
                    var firstCharacter = input.FirstOrDefault();
                    if (int.TryParse(firstCharacter.ToString(), out var number))
                    {
                        elevatorService.PressButton(elevator, number);
                    }
                }
            }
        }
    }
}