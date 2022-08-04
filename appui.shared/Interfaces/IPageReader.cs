using HtmlAgilityPack;

namespace appui.shared.Interfaces
{
    public interface IPageReader
    {
        Task<HtmlDocument> LoadPageAsync();
    }
}
