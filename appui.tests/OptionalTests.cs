using appui.shared.Utils;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace appui.tests
{
    public class OptionalTests
    {
        [Fact]
        public void OptionalEmpty_ListOfObjects_NotNullAndEmpty()
        {
            var emptyList = Optional.Empty<List<object>>();
            Assert.NotNull(emptyList);
            Assert.Empty(emptyList);
        }

        [Fact]
        public void OptionalEmpty_String_EmptyString()
        {
            var emptyString = Optional.Empty();
            Assert.Same(string.Empty, emptyString);
        }

        [Fact]
        public void OptionalEmpty_Object_EmptyObject()
        {
            var emptyObject = Optional.Empty<object>();
            Assert.NotNull(emptyObject);
        }
    }
}