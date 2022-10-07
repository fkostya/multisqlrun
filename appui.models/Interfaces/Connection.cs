namespace appui.models.Interfaces
{
    public abstract class Connection
    {
        public string? Name { get; set; }
        public abstract T GetConnectionString<T>();
    }
}