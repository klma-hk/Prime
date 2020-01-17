using System;
using System.Windows.Input;

namespace WPFMT
{
    public class RelayCommand : RelayCommandBase, ICommand
    {
        private readonly Action execute;
        public RelayCommand(Action execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            if (canExecute != null)
            {
                this.canExecute = canExecute;
            }
        }
        public void Execute(object parameter)
        {
            execute();
        }
    }
}
