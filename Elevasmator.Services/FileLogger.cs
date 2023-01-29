using Elevasmator.Services.Models;
using Microsoft.Extensions.Configuration;

namespace Elevasmator.Services
{
    public class FileLogger : ILogger
    {
        private static object lockObject = new object();
        private readonly string? directory;
        private readonly string fileName;

        public FileLogger(IConfiguration configuration)
        {
            this.directory = configuration["LogFilePath"];
            this.fileName = Path.Combine(this.directory, $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log");
        }

        public bool TestFilePath()
        {
            if (Directory.Exists(this.directory))
            {
                if (!File.Exists(this.fileName))
                {
                    File.Create(this.fileName);
                }

                return true;
            }

            return false;
        }

        public void Write(string log)
        {
            lock (lockObject)
            {
                File.AppendAllText(this.fileName, $"{DateTime.Now}: {log} \r\n");
            }
        }

        public void WriteButtonPress(int floor, ButtonType buttonType)
        {
            var buttonLocationText = buttonType == ButtonType.Internal ? "Inside" : "Outside";
            var logText = $"{buttonLocationText} button pressed for floor {floor}.";

            if (buttonType == ButtonType.ExternalUp)
            {
                logText += " Going UP.";
            }
            else if (buttonType == ButtonType.ExternalDown)
            {
                logText += " Going DOWN.";
            }

            this.Write(logText);
        }

        public void WriteFloorPass(int floor)
        {
            this.Write($"Passing floor {floor}");
        }

        public void WriteFloorStop(int floor)
        {
            this.Write($"Stopping at floor {floor}");
        }
    }
}