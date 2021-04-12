namespace BrainstormSessions.Core.Interfaces
{
    public interface ILogger
    {
        void Debug(object message);
        void Error(object message);
        void Info(object message);
        void Warn(object message);
        void Fatal(object message);
    }
}
