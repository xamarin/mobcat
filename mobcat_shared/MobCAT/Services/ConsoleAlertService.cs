using System;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.Services
{
    internal class ConsoleAlertService : IAlertService
    {
        public Task<string> DisplayActionSheetAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string destructionButton = null,
            params string[] actions)
        {
            Logger.Debug($"[Alerts] Display action sheet alert - with title: {title}");
            return Task.FromResult((string)null);
        }

        /// <inheritdoc />
        public Task DisplayAsync(
            string title,
            string message = null,
            string cancelButton = null)
        {
            Logger.Debug($"[Alerts] Display alert - with title: {title}");
            return Task.FromResult((string)null);
        }

        /// <inheritdoc />
        public Task<bool> DisplayConfirmationAsync(
            string title,
            string message = null,
            string cancelButton = null,
            string actionButton = null)
        {
            Logger.Debug($"[Alerts] Display alert - action button with with title: {title}");
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task<DateTime?> DisplayDatePickerAsync(
            string title = null,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            Logger.Debug($"[Alerts] Display date picker - with title: {title}");
            return Task.FromResult((DateTime?)null);
        }

        /// <inheritdoc />
        public Task<string> DisplayInputEntryAsync(
            string title,
            string message = null,
            string actionButton = null,
            string cancelButton = null,
            string hint = null,
            Func<string, bool> validator = null)
        {
            Logger.Debug($"[Alerts] Display input entry - with title: {title}");
            return Task.FromResult((string)null);
        }
    }
}
