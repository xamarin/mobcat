using System;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT
{
    public static class Logger
    {
        private static ILoggingService _registeredService;
        private static Func<ILoggingService> _registeredServiceFunc;

        /// <summary>
        /// Gets the currently registered ILoggingService.
        /// </summary>
        /// <value>The current service.</value>
        public static ILoggingService RegisteredService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<ILoggingService>(true);

                    if (_registeredService == null)
                    {
                        _registeredService = new ConsoleLoggingService();
                        _registeredService.Log(LogType.WARNING, "No logging service was registered.  Falling back to console logging.");
                    }
                }

                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the ILoggingService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(ILoggingService service)
        {
            ServiceContainer.Register<ILoggingService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Lazily registers the ILoggingService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(Func<ILoggingService> service)
        {
            ServiceContainer.Register<ILoggingService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Write to debug log
        /// </summary>
        /// <param name="parameters">Parameters to write.</param>
        public static void Debug(params object[] parameters)
        {
            Log(LogType.DEBUG, parameters);
        }

        /// <summary>
        /// Write to warning log
        /// </summary>
        /// <param name="parameters">Parameters to write.</param>
        public static void Warn(params object[] parameters)
        {
            Log(LogType.WARNING, parameters);
        }

        /// <summary>
        /// Write to error log
        /// </summary>
        /// <param name="parameters">Parameters to write.</param>
        public static void Error(params object[] parameters)
        {
            Log(LogType.ERROR, parameters);
        }

        /// <summary>
        /// Write to fatal log
        /// </summary>
        /// <param name="parameters">Parameters to write.</param>
        public static void Fatal(params object[] parameters)
        {
            Log(LogType.FATAL, parameters);
        }

        /// <summary>
        /// Write to info log
        /// </summary>
        /// <param name="parameters">Parameters to write.</param>
        public static void Info(params object[] parameters)
        {
            Log(LogType.INFO, parameters);
        }

        /// <summary>
        /// Write to specified LogType
        /// </summary>
        /// <returns>The log.</returns>
        /// <param name="logType">Log type.</param>
        /// <param name="parameters">Parameters to write.</param>
        public static void Log(LogType logType, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return;

            if (parameters.Length == 1)
            {
                if (parameters[0] is Exception exception)
                {
                    RegisteredService.Log(logType, exception.Message, exception);
                    return;
                }

                var value = parameters[0];
                if (value != null)
                {
                    RegisteredService.Log(logType, value.ToString());
                    return;
                }
            }

            var format = parameters[0] != null ? parameters[0].ToString() : "";
            var message = format;

            try
            {
                var args = new object[parameters.Length - 1];
                Array.Copy(parameters, 1, args, 0, parameters.Length - 1);

                message = string.Format(format, args);
            }
            catch (Exception exc)
            {
                RegisteredService.Log(LogType.INFO, string.Format("An error occured formatting the logging message: [{0}]", format), exc);
            }

            if (parameters[parameters.Length - 1] is Exception ex)
            {
                RegisteredService.Log(logType, message, ex);
            }
            else
            {
                RegisteredService.Log(logType, message);
            }
        }
    }
}