using System;

namespace QuantumAlgorithms.Common
{
    public interface ILogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string message) =>
            Log("  INFO     ", message);

        public void Warning(string message) =>
            Log("  WARNING  ", message);

        public void Error(string message) =>
            Log("  ERROR    ", message);

        private void Log(string severityString, string message) =>
            Console.WriteLine(DateTime.Now + severityString + message);
    }
}
