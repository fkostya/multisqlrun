using appui.shared.Interfaces;

namespace appui.shared
{
    public class EmptyConnector : IConnector
    {
        public bool Offline { get; set; } = true;

        public Task<int> Load()
        {
            return Task.FromResult(0);
        }
    }
}
