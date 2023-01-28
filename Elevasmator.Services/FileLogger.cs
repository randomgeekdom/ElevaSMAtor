using Microsoft.Extensions.Configuration;

namespace Elevasmator.Services
{
    public class FileLogger : ILogger
    {
        private readonly string fileName;
        private readonly string? directory;

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
            File.AppendAllText(this.fileName, $"{DateTime.Now}: {log} \r\n");
        }
    }
}