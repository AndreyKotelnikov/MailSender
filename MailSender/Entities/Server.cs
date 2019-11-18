using Entities.Abstract;

namespace Entities
{
    public class Server : ConnectionEntity
    {
        public int Port { get; set; }

        public bool UseSSL { get; set; } = true;

        public string UserName { get; set; }
    }
}
