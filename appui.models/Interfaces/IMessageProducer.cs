namespace appui.models.Interfaces
{
    public interface IMessageProducer
    {
        void Publish(IMessagePayload message);
    }
}
