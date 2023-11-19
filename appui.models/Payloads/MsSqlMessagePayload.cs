using appui.models.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.models.Payloads
{
    [ExcludeFromCodeCoverage]

    public class MsSqlMessagePayload : IMessagePayload
    {
        public MsSqlConnection? Connection { get; set; }
        public string? StoragePath { get; set; }//REFACTOR: remove after switching to rabbitmq 
    }
}