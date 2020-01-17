using System;
using System.Windows.Input;

namespace WPFMT
{
    public class RelayCommand<T> : RelayCommandBase, ICommand
    {
        private readonly Action<T> execute;
        public RelayCommand(Action<T> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                
                execute((T)parameter);
            }
            else
            {
                throw new ArgumentException("Parameter cannot be null", "parameter");
            }

           
        }

    }
}
