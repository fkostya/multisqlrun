namespace appui.models.Interfaces
{
    public abstract class Tenant<T> 
        where T : Connection
    {
        public string? Name { get; set; }
        public T? Connection { get; set; }
    }
}
