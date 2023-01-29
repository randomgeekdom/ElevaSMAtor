using Elevasmator.Services.Models;

namespace Elevasmator.Services
{
    public interface ILogger
    {
        bool TestFilePath();

        void Write(string log);

        void WriteButtonPress(int floor, ButtonType buttonType);

        void WriteFloorPass(int floor);

        void WriteFloorStop(int floor);
    }
}