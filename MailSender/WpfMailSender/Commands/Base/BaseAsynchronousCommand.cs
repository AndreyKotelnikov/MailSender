using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace WpfMailSender.Commands.Base
{
    public abstract class BaseAsynchronousCommand : BaseCommand, INotifyPropertyChanged
    {
        protected Dispatcher CallingDispatcher;
        private bool _isExecuting;
        private bool _isCancellationRequested;
        private bool _disableDuringExecuting;

        protected BaseAsynchronousCommand(Action action, bool canExecute = true)
          : base(action, canExecute)
        {
            Initialise();
        }

        protected BaseAsynchronousCommand(Action<object> parameterizedAction, bool canExecute = true)
          : base(parameterizedAction, canExecute)
        {
            Initialise();
        }

        private void Initialise()
        {
            CancelCommand = new CancelCommand(() => IsCancellationRequested = true, true);
        }

        public override void DoExecute(object param)
        {
            if (IsExecuting)
                return;
            CancelCommandEventArgs commandEventArgs = new CancelCommandEventArgs
            {
                Parameter = param,
                Cancel = false
            };
            CancelCommandEventArgs args = commandEventArgs;
            InvokeExecuting(args);
            if (args.Cancel)
                return;
            IsExecuting = true;
            if (DisableDuringExecution)
                CanExecute = false;
            CallingDispatcher = DispatcherHelper.CurrentDispatcher;
            ThreadPool.QueueUserWorkItem(state =>
            {
                InvokeAction(param);
                ReportProgress(() =>
                {
                    IsExecuting = false;
                    if (IsCancellationRequested)
                        InvokeCancelled(new CommandEventArgs()
                        {
                            Parameter = param
                        });
                    else
                        InvokeExecuted(new CommandEventArgs()
                        {
                            Parameter = param
                        });
                    IsCancellationRequested = false;
                    if (!DisableDuringExecution)
                        return;
                    CanExecute = true;
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
                return;
            if (CallingDispatcher.CheckAccess())
                progressAction();
            else
                CallingDispatcher.BeginInvoke(progressAction);
        }

        public bool CancelIfRequested()
        {
            return IsCancellationRequested;
        }

        protected void InvokeCancelled(CommandEventArgs args)
        {
            CommandEventHandler cancelled = Cancelled;
            cancelled?.Invoke(this, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event CommandEventHandler Cancelled;

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (_isExecuting == value)
                    return;
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
                    return;
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
                    return;
                _disableDuringExecuting = value;
                NotifyPropertyChanged(nameof(DisableDuringExecution));
            }
        }

        public BaseCommand CancelCommand { get; private set; }
    }
}
