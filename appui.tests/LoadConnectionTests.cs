using appui.shared.Interfaces;
using HtmlAgilityPack;

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

        [Fact]
        [Trait("Category", "Shared")]
        public void UnitIfWork_InitialCondition_ExpectedResult()
        {
            //Arrange
            //Act

        }
    }
}