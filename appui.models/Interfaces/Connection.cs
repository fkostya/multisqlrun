using System.Diagnostics.CodeAnalysis;

namespace appui.models.Interfaces
{
    [ExcludeFromCodeCoverage]
    public abstract class Connection
    {
        public string? ConnectionName { get; set; }

        public abstract T GetConnectionString<T>();

        public abstract bool IsValid();
    }
}