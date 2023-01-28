using Elevasmator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevasmator
{
    internal class Program
    {
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(
                    (_, services) => services
                        .AddTransient<ElevatorOperationService>()
                        .AddTransient<ILogger, FileLogger>()
                        .AddTransient<IElevatorFactory, ElevatorFactory>()
                        .AddTransient<IElevatorMovementService, ElevatorMovementService>()
                        .AddSingleton<IElevatorService, ElevatorService>());
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var operationService = host.Services.GetRequiredService<ElevatorOperationService>();
            var elevatorService = host.Services.GetRequiredService<IElevatorService>();
            var elevatorFactory = host.Services.GetRequiredService<IElevatorFactory>();

            var tokenSource = new CancellationTokenSource();

            var elevator = elevatorFactory.Create();
            var sensor = new Sensor();

            Task.Factory.StartNew(() => operationService.StartupElevator(elevator, sensor, tokenSource.Token));

            var logger = host.Services.GetRequiredService<ILogger>();
            if (logger.TestFilePath())
            {
                while (true)
                {
                    Console.Write("BUTTON PRESS ==> ");
                    var input = Console.ReadLine();

                    input = input?.ToLower().Trim();

                    if (input == "q")
                    {
                        tokenSource.Cancel();
                        break;
                    }

                    if (input != null)
                    {
                        var numberString = new string(input.Where(x => char.IsNumber(x)).ToArray());
                        if (int.TryParse(numberString, out var number))
                        {
                            elevatorService.PressButton(elevator, number);
                            continue;
                        }
                    }

                    Console.WriteLine("Invalid input");
                }
            }
            else
            {
                Console.WriteLine("Invalid configured file path");
            }
            
        }
    }
}