using appui.models.Interfaces;

namespace appui.models.Payloads
{
    public class MsSqlMessagePayload : IMessagePayload
    {
        public MsSqlConnection? Connection { get; set; }
        public string? Query { get; set; }
        public string? StoragePath { get; set; }//REFACTOR: remove after switching to rabbitmq 
    }
}