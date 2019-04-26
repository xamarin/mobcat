using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT
{
    public static class Alerts
    {
        private static IAlertService _registeredService;
        private static Func<IAlertService> _registeredServiceFunc;

        public static IAlertService RegisteredService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IAlertService>(true);
                    if (_registeredService == null)
                    {
                        _registeredService = new ConsoleAlertService();
                        Logger.Warn("No alert service was registered.  Falling back to console alerts.");
                    }
                }
                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the IAlertService instance.
        /// </summary>
        /// <param name="service">IAlertService implementation.</param>
        public static void RegisterService(IAlertService service)
        {
            ServiceContainer.Register<IAlertService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Registers the IAlertService instance.
        /// </summary>
        /// <param name="service">IAlertService implementation.</param>
        public static void RegisterService(Func<IAlertService> service)
        {
            ServiceContainer.Register<IAlertService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Displays an action sheet.
        /// </summary>
        /// <returns>A task with result of selected action.</returns>
        /// <param name="title">Title.</param>
        /// <param name="cancelButton">Cancel.</param>
        /// <param name="destructionButton">Destruction.</param>
        /// <param name="actions">Actions.</param>
        public static Task<string> DisplayActionSheetAsync(
           string title,
           string message = null,
           string cancelButton = null,
           string destructionButton = null,
           params string[] actions)
        {
            return RegisteredService.DisplayActionSheetAsync(
                title,
                message,
                cancelButton,
                destructionButton,
                actions);
        }

        /// <summary>
        /// Displays a dialog with a given title, message and a cancel button label
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="cancelButton">Cancel button label</param>
        /// <returns>An instance of a task</returns>
        public static Task DisplayAsync(
            string title,
            string message = null,
            string cancelButton = null)
        {
            return RegisteredService.DisplayAsync(
                title,
                message,
                cancelButton);
        }

        /// <summary>
        /// Displays a dialog with a given title, message, an accept button label and a cancel button label 
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="actionButton">Accept button label</param>
        /// <param name="cancelButton">Cancel button label</param>
        /// <returns>An instance of a task</returns>
        public static Task<bool> DisplayConfirmationAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string actionButton = null)
        {
            return RegisteredService.DisplayConfirmationAsync(
                title,
                message,
                cancelButton,
                actionButton);
        }

        /// <summary>
        /// Displays the date picker async.
        /// </summary>
        /// <returns>Task with result of the entered date.</returns>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="actionButton">Action button.</param>
        /// <param name="cancelButton">Cancel button.</param>
        /// <param name="initialDate">Initial date.</param>
        /// <param name="minDate">Minimum date.</param>
        /// <param name="maxDate">Max date.</param>
        public static Task<DateTime?> DisplayDatePickerAsync(
            string title = null,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            return RegisteredService.DisplayDatePickerAsync(
                title,
                message,
                actionButton,
                cancelButton,
                initialDate,
                minDate,
                maxDate);
        }

        /// <summary>
        /// Displays an input entry async.
        /// </summary>
        /// <returns>Task with result of the entered string </returns>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="actionButton">Action button.</param>
        /// <param name="cancelButton">Cancel button.</param>
        /// <param name="hint">Hint.</param>
        /// <param name="validator">Validator.</param>
        public static Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null)
        {
            return RegisteredService.DisplayInputEntryAsync(
                title,
                message,
                actionButton,
                cancelButton,
                hint,
                validator);
        }
    }
}
