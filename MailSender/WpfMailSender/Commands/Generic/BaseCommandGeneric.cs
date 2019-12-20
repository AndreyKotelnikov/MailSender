using System;
using System.Windows.Input;

namespace WpfMailSender.Commands.Generic
{
    public abstract class BaseCommandGeneric<TParameter> : ICommand
    {
        protected Action<TParameter> ParameterizedAction;
        private bool _canExecute;

        protected BaseCommandGeneric() : this(null, true)
        {
        }

        protected BaseCommandGeneric(Action<TParameter> parameterizedAction, bool canExecute = true)
        {
            ParameterizedAction = parameterizedAction;
            _canExecute = canExecute;
        }

        public virtual void DoExecute(TParameter param)
        {
            CancelCommandEventArgsGeneric<TParameter> commandEventArgs =
                new CancelCommandEventArgsGeneric<TParameter>
                {
                    Parameter = param, 
                    Cancel = false
                };
            CancelCommandEventArgsGeneric<TParameter> args = commandEventArgs;
            InvokeExecuting(args);
            if (args.Cancel)
            {
                return;
            }
               
            param = args.Parameter;
            InvokeAction(param);
            InvokeExecuted(new CommandEventArgsGeneric<TParameter>()
            {
                Parameter = param
            });
        }

        protected void InvokeAction(TParameter param)
        {
            Action<TParameter> parameterizedAction = ParameterizedAction;
            parameterizedAction?.Invoke(param);
        }

        protected void InvokeExecuted(CommandEventArgsGeneric<TParameter> args)
        {
            CommandEventHandlerGeneric<TParameter> executed = Executed;
            executed?.Invoke(this, args);
        }

        protected void InvokeExecuting(CancelCommandEventArgsGeneric<TParameter> args)
        {
            CancelCommandEventHandlerGeneric<TParameter> executing = Executing;
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
            if (!(parameter is TParameter))
                throw new InvalidOperationException("A parameter of type " + (parameter != null ? parameter.GetType() : typeof(object)).Name + " was passed to a Command expecting a parameter of type " + typeof(TParameter).Name + ". Check the binding of the 'CommandParameter'.");
            DoExecute((TParameter)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public event CancelCommandEventHandlerGeneric<TParameter> Executing;

        public event CommandEventHandlerGeneric<TParameter> Executed;
    }
}
