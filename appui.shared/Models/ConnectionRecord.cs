using appui.shared.Interfaces;

namespace appui.shared.Models
{
    public class ConnectionRecord : IConnectionRecord
    {
        public string id { get; set; }
        public string client { get; set; }
        public string database { get; set; }
        public string server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }
}
