using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMailSender.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected Action Action;
        protected Action<object> ParameterizedAction;
        private bool _canExecute;

        protected BaseCommand(Action action, bool canExecute = true)
        {
            Action = action;
            _canExecute = canExecute;
        }

        protected BaseCommand(Action<object> parameterizedAction, bool canExecute = true)
        {
            ParameterizedAction = parameterizedAction;
            _canExecute = canExecute;
        }

        public virtual void DoExecute()
        {
            DoExecute(null);
        }

        public virtual void DoExecute(object param)
        {
            CancelCommandEventArgs commandEventArgs = new CancelCommandEventArgs
            {
                Parameter = param,
                Cancel = false
            };
            CancelCommandEventArgs args = commandEventArgs;
            InvokeExecuting(args);
            if (args.Cancel)
                return;
            param = args.Parameter;
            InvokeAction(param);
            InvokeExecuted(new CommandEventArgs()
            {
                Parameter = param
            });
        }

        protected void InvokeAction(object param)
        {
            Action action = Action;
            Action<object> parameterizedAction = ParameterizedAction;
            if (action != null)
            {
                action();
            }
            else
            {
                if (parameterizedAction == null)
                    return;
                parameterizedAction(param);
            }
        }

        protected void InvokeExecuted(CommandEventArgs args)
        {
            CommandEventHandler executed = Executed;
            executed?.Invoke(this, args);
        }

        protected void InvokeExecuting(CancelCommandEventArgs args)
        {
            CancelCommandEventHandler executing = Executing;
            executing?.Invoke(this, args);
        }

        public bool CanExecute
        {
            get => _canExecute;
            set
            {
                if (_canExecute == value)
                    return;
                _canExecute = value;
                EventHandler canExecuteChanged = CanExecuteChanged;
                canExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            DoExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public event CancelCommandEventHandler Executing;

        public event CommandEventHandler Executed;
    }
}
