using appui.shared.Interfaces;
using HtmlAgilityPack;

namespace appui.tests
{
    public class PageParserTests
    {
        class FakePageReader : IPageReader
        {
            public Task<HtmlDocument> GetPageAsync(string url)
            {
                throw new NotImplementedException();
            }
        }
        [Fact]
        public void Parse()
        {
            Assert.False(1 == 0);
        }
    }
}