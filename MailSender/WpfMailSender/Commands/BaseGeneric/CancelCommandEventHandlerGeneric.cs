namespace WpfMailSender.Commands.BaseGeneric
{
    public delegate void CancelCommandEventHandlerGeneric<TParameter>(
        object sender,
        CancelCommandEventArgsGeneric<TParameter> args);
}
