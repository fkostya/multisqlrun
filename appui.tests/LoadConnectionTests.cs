using appui.shared.Interfaces;
using HtmlAgilityPack;
using NUnit.Framework;
using Telerik.JustMock;

namespace appui.tests
{
    //UnitIfWork_InitialCondition_ExpectedResult
    //Example: UserLogsIn_WithValidCredetntials_RedirectsToHome
    public class LoadConnectionTests
    {
        public LoadConnectionTests()
        {

        }
        class FakePageReader : IPageReader
        {
            public Task<HtmlDocument> GetPageAsync()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        [Category("Shared")]
        public void UnitIfWork_InitialCondition_ExpectedResult()
        {
        }
    }
}