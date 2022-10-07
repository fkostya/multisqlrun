using appui.models.Interfaces;

namespace appui.consumers.Interfaces
{
    public interface IMessageProducer
    {
        void Publish(IMessagePayload message);
    }
}
