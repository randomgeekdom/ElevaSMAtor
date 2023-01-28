using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevasmator.Services
{
    public class FileLogger : ILogger
    {
        private readonly string fileName;

        public FileLogger()
        {
            this.fileName = $"C:\\logs\\{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log";
        }

        public void Write(string log)
        {
            File.AppendAllText(this.fileName, $"{DateTime.Now}: {log} \r\n");
        }
    }
}
