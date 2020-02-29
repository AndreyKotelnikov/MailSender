namespace WpfMailSender.Commands.BaseGeneric
{
    public class CancelCommandEventArgsGeneric<TParameter> : CommandEventArgsGeneric<TParameter>
    {
        public bool Cancel { get; set; }
    }
}
