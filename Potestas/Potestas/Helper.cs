using Potestas.Utils;
using System;

namespace Potestas
{
    public static class Helper
    {
        public static T Run<T>(Func<T> function, ILoggerManager _loggerManager, string methodName)
        {
            try
            {
                _loggerManager.LogInfo($"Method {methodName} has started to work.");

                var result = function();

                _loggerManager.LogInfo($"Method {methodName} has ended to work.");

                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"Method {methodName} has exited with the exception: {ex}.");

                throw;
            }
        }

        public static void Run(Action function, ILoggerManager _loggerManager, string methodName)
        {
            try
            {
                _loggerManager.LogInfo($"Method {methodName} has started to work.");

                function();

                _loggerManager.LogInfo($"Method {methodName} has ended to work.");
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"Method {methodName} has exited with the exception: {ex}.");

                throw;
            }
        }
    }
}
