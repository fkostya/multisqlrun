namespace appui.models.Interfaces
{
    public abstract class Connection
    {
        public string? ConnectionName { get; set; }
        public abstract T GetConnectionString<T>();
    }
}