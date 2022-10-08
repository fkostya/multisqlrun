namespace appui.models.Interfaces
{
    public abstract class CatalogContext
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Tenant<Connection>>? Tenants { get; set; }
    }
}