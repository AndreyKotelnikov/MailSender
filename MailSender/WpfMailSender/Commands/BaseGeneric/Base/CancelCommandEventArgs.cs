namespace WpfMailSender.Commands.BaseGeneric.Base
{
    public class CancelCommandEventArgs : CommandEventArgs
    {
        public bool Cancel { get; set; }
    }
}
