using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BindingTest
{
    public class MyCommand : ICommand
    {
        Action executeAction;
        public MyCommand(Action action)
        {
            executeAction = action;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            executeAction();
        }
    }
}
