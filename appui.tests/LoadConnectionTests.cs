using appui.shared;
using appui.shared.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace appui.tests
{
    class PageReaderStub : IPageReader
    {
        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html></html>");

            return await Task.FromResult(doc);
        }
    }

    class PageReaderFactoryStub : IPageReaderFactory
    {
        public IPageReader CreatePageReader()
        {
            return new PageReaderStub();
        }
    }
    //UnitIfWork_InitialCondition_ExpectedResult
    //Example: UserLogsIn_WithValidCredetntials_RedirectsToHome
    [TestFixture]
    public class LoadConnectionTests
    {
        public LoadConnectionTests()
        {

        }

        [TestCase]
        public void UnitIfWork_InitialCondition_ExpectedResult()
        {
            //Options.Create(new SampleOptions());

            //var mock = new Mock<IPageReaderFactory>()
            //    .Setup(c => c.CreatePageReader())
            //    .Returns(new MyClass());

            LoadConnections con= new LoadConnections(new PageReaderFactoryStub());
            var doc = con.Load();
            
            Assert.IsNotNull(doc);
        }
    }
}