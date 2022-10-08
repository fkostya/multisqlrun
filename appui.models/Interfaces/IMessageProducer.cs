namespace appui.models.Interfaces
{
    public interface IMessageProducer
    {
        Task Publish(IMessagePayload message);
    }
}
