using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Demo_NTier_WpfPresentation
{
    class RelayCommand : ICommand
    {
        private Action<object> _actionP1;

        private Action _action;

        public RelayCommand(Action<object> action)
        {
            _actionP1 = action;
        }

        public RelayCommand(Action action)
        {
            _action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                _actionP1(parameter);
            }
            else
            {
                _action();
            }
        }

        #endregion
    }
}
