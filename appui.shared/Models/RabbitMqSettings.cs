using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqSettings
    {
        public string Uri { get; set; }
        public string Queue { get; set; }
        public string Exchange { get; set; }
    }
}