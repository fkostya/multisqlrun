using appui.shared.Interfaces;
using HtmlAgilityPack;

namespace appui.tests
{
    public class LoadConnectionTests
    {
        class FakePageReader : IPageReader
        {
            public Task<HtmlDocument> GetPageAsync(string url)
            {
                throw new NotImplementedException();
            }
        }
        [Fact]
        public void Load()
        {
            Assert.False(1 == 0);
        }
    }
}