namespace Elevasmator.Services
{
    public interface ILogger
    {
        void Write(string log);
        void WriteFloorStop(int floor);
        void WriteFloorPass(int floor);
        bool TestFilePath();
    }
}