using System;
using System.Windows.Input;

namespace Microsoft.MobCAT.MVVM
{
    public sealed class Command<T> : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        public Command(Action<T> execute) : base(o => execute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        /// <param name="canExecute">Action to determine whether the command can execute or not.</param>
        public Command(Action<T> execute, Func<T, bool> canExecute) : base(o => execute((T)o), o => canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }
    }

    public class Command : ICommand
    {
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        public Command(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        public Command(Action execute) : this(o => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        /// <param name="canExecute">Func to determine whether the command can execute or not.</param>
        public Command(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.MobCat.Mvvm.Command"/> class.
        /// </summary>
        /// <param name="execute">Action to call when Execute is invoked.</param>
        /// <param name="canExecute">Func to determine whether the command can execute or not.</param>
        public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);

            return true;
        }

        public event EventHandler CanExecuteChanged;

        /// <inheritdoc />
        public void Execute(object parameter) => _execute(parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event if there is a handler.
        /// </summary>
        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}