using Prism.Logging;

namespace GlossSensorTest.DatabaseManager
{
    public interface ILoggerFacade<T>
    {
        void Log(string message, Category category, Priority priority);
    }
}