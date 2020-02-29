using System;
using System.Windows.Input;

namespace WpfMailSender.Commands.BaseGeneric
{
    public abstract class BaseCommandGeneric<TParameter> : ICommand
    {
        protected Action<TParameter> ParameterizedAction;
        protected Func<TParameter, bool> CanExecuteFunc;
        protected bool CanExecuteManualSet = true;

        protected BaseCommandGeneric() : this(null, null)
        {
        }

        protected BaseCommandGeneric(Action<TParameter> parameterizedAction, Func<TParameter, bool> canExecuteFunc)
        {
            ParameterizedAction = parameterizedAction;
            CanExecuteFunc = canExecuteFunc;
        }

        protected virtual void DoExecute(TParameter param)
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

        bool ICommand.CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
               
            if (!(parameter is TParameter))
                throw new InvalidOperationException($"A parameter of type { parameter.GetType() } was passed to a Command expecting a parameter of type { typeof(TParameter).Name }. " +
                                                    "Check the binding of the 'CommandParameter'.");
            
            return CanExecuteManualSet 
                   && (CanExecuteFunc?.Invoke((TParameter)parameter) 
                       ?? true);
        }

        void ICommand.Execute(object parameter)
        {
            if (!(parameter is TParameter))
                throw new InvalidOperationException($"A parameter of type { parameter.GetType() } was passed to a Command expecting a parameter of type { typeof(TParameter).Name }. " +
                                                    "Check the binding of the 'CommandParameter'.");
            DoExecute((TParameter)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public event CancelCommandEventHandlerGeneric<TParameter> Executing;

        public event CommandEventHandlerGeneric<TParameter> Executed;
    }
}
