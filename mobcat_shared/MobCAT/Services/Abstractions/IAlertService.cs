using System;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.Services
{
    public interface IAlertService
    {
        /// <summary>
        /// Displays a dialog with a given title, message and a cancel button label
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="cancelButton">Cancel button label</param>
        /// <returns>An instance of a task</returns>
        Task DisplayAsync(
            string title,
            string message = null,
            string cancelButton = null);

        /// <summary>
        /// Displays a confirmation dialog with a given title, message, an accept button label and a cancel button label 
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="actionButton">Accept button label</param>
        /// <param name="cancelButton">Cancel button label</param>
        /// <returns>An instance of a task</returns>
        Task<bool> DisplayConfirmationAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string actionButton = null);

        /// <summary>
        /// Displays an action sheet.
        /// </summary>
        /// <returns>A task with result of selected action.</returns>
        /// <param name="title">Title.</param>
        /// <param name="cancelButton">Cancel.</param>
        /// <param name="destructionButton">Destruction.</param>
        /// <param name="actions">Actions.</param>
        Task<string> DisplayActionSheetAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string destructionButton = null,
            params string[] actions);

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
        Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null);

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
        Task<DateTime?> DisplayDatePickerAsync(
            string title = null,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null);
    }

}
