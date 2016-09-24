using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualProgrGUI.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Do when comand executes
        /// </summary>
        Action<object> _execute;

        public SimpleCommand(Action<object> excecute)
        {
            _execute = excecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
