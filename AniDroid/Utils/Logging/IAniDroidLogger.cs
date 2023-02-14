using System;

namespace AniDroidv2.Utils.Logging
{
    public interface IAniDroidv2Logger
    {
        void Debug(string tag, string message);
        void Info(string tag, string message);
        void Warning(string tag, string message);
        void Error(string tag, string message, Exception exception = null);
    }
}