using appui.shared;
using appui.shared.Interfaces;
using Moq;
using Xunit;

namespace appui.tests.Shared
{
    public class FileUtilityTests
    {
        [Fact]
        public void GenerateUniqueStorageName_RootFolderIsEmpty_ApplicationFolder()
        {
            var directoryWrapperMock = new Mock<DirectoryWrapper>();
            var fu = new FileUtility(directoryWrapperMock.Object);

            var folderPath = fu.GenerateUniqueStorageName(string.Empty);
            Assert.StartsWith(".\\", folderPath);
        }


        [Fact]
        public void GenerateUniqueStorageName_RootFolderCustomName_NameStartsWithCustomName()
        {
            var directoryWrapperMock = new Mock<DirectoryWrapper>();
            var fu = new FileUtility(directoryWrapperMock.Object);

            var folderPath = fu.GenerateUniqueStorageName("custom-name");
            Assert.StartsWith(".\\custom-name", folderPath);
        }

        [Fact]
        public void CreateDirectory_PathIsEmpty_ThrowException()
        {
            var dwMock = new Mock<IDirectoryWrapper>();
            dwMock.Setup(f => f.CreateDirectory(It.IsAny<string>())).Throws<Exception>();
            
            var fu = new FileUtility(dwMock.Object);
            Assert.Throws<Exception>(() => fu.CreateStorage(string.Empty));

            //Assert.CatchAsync<Exception>(async () => await )
        }

        [Fact]
        public void CreateDirectory_PathName_NotNullDirectoryInfo()
        {
            var dwMock = new Mock<IDirectoryWrapper>();
            dwMock.Setup(f => f.CreateDirectory(It.IsAny<string>())).Returns<string>((path) => new DirectoryInfo("directory-name"));

            var fu = new FileUtility(dwMock.Object);
            Assert.NotNull(fu.CreateStorage("directory-name"));
        }
    }
}