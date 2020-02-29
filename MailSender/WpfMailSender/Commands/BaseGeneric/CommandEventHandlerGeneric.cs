namespace WpfMailSender.Commands.BaseGeneric
{
    public delegate void CommandEventHandlerGeneric<TParameter>(
        object sender,
        CommandEventArgsGeneric<TParameter> args);
}
