namespace WpfMailSender.Commands.Base
{
    public class CancelCommandEventArgs : CommandEventArgs
    {
        public bool Cancel { get; set; }
    }
}
