using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfMailSender.Commands.Base;

namespace WpfMailSender.Commands.Generic
{
    public class AsynchronousCommandGeneric<TParameter> : BaseCommandGeneric<TParameter>, INotifyPropertyChanged
    {
        protected Dispatcher CallingDispatcher;
        private bool _isExecuting;
        private bool _isCancellationRequested;
        private bool _disableDuringExecuting;


        public AsynchronousCommandGeneric() : this(null, null)
        {
        }

        public AsynchronousCommandGeneric(Action<TParameter> parameterizedAction, Func<TParameter, bool> canExecuteFunc)
          : base(parameterizedAction, canExecuteFunc)
        {
            Initialise();
        }

        private void Initialise()
        {
            CancelCommand = new CancelCommandGeneric<TParameter>(((parameter) => IsCancellationRequested = true), null);
        }

        protected override void DoExecute(TParameter param)
        {
            if (IsExecuting)
                return;
            CancelCommandEventArgsGeneric<TParameter> commandEventArgs = new CancelCommandEventArgsGeneric<TParameter>()
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
                
            IsExecuting = true;
            if (DisableDuringExecution)
            {
                CanExecuteManualSet = false;
            }

            CallingDispatcher = DispatcherHelper.CurrentDispatcher;
            ThreadPool.QueueUserWorkItem(state =>
            {
                InvokeAction(param);
                ReportProgress(() =>
                {
                    IsExecuting = false;
                    if (IsCancellationRequested)
                        InvokeCancelled(new CommandEventArgsGeneric<TParameter>()
                        {
                            Parameter = param
                        });
                    else
                        InvokeExecuted(new CommandEventArgsGeneric<TParameter>()
                        {
                            Parameter = param
                        });
                    IsCancellationRequested = false;
                    if (!DisableDuringExecution)
                    {
                        CanExecuteManualSet = true;
                    }
                });
            });
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReportProgress(Action progressAction)
        {
            if (!IsExecuting)
            {
                return;
            }
                
            if (CallingDispatcher.CheckAccess())
                progressAction();
            else
                CallingDispatcher.BeginInvoke(progressAction);
        }

        public bool CancelIfRequested()
        {
            return IsCancellationRequested;
        }

        protected void InvokeCancelled(CommandEventArgsGeneric<TParameter> args)
        {
            CommandEventHandlerGeneric<TParameter> cancelled = Cancelled;
            cancelled?.Invoke(this, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event CommandEventHandlerGeneric<TParameter> Cancelled;

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (_isExecuting == value)
                {
                    return;
                }
                    
                _isExecuting = value;
                NotifyPropertyChanged(nameof(IsExecuting));
            }
        }

        public bool IsCancellationRequested
        {
            get => _isCancellationRequested;
            set
            {
                if (_isCancellationRequested == value)
                {
                    return;
                }
                    
                _isCancellationRequested = value;
                NotifyPropertyChanged(nameof(IsCancellationRequested));
            }
        }

        public bool DisableDuringExecution
        {
            get => _disableDuringExecuting;
            set
            {
                if (_disableDuringExecuting == value)
                {
                    return;
                }
                    
                _disableDuringExecuting = value;
                NotifyPropertyChanged(nameof(DisableDuringExecution));
            }
        }

        public BaseCommandGeneric<TParameter> CancelCommand { get; private set; }
    }
}
