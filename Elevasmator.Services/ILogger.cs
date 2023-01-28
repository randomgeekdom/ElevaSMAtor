namespace Elevasmator.Services
{
    public interface ILogger
    {
        void Write(string log);
        bool TestFilePath();
    }
}