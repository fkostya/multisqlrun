using appui.shared.Interfaces;

namespace appui.shared
{
    public class FileUtility : IStorageUtility
    {
        private readonly IDirectoryWrapper _directoryWrapper;

        public FileUtility(IDirectoryWrapper directoryWrapper)
        {
            this._directoryWrapper = directoryWrapper;
        }

        public DirectoryInfo CreateStorage(string path)
        {
            return _directoryWrapper.CreateDirectory(path);
        }

        public string GenerateUniqueStorageName(string rootStorageName)
        {
            var rootFolder = string.IsNullOrEmpty(rootStorageName) ? string.Empty : $"\\{rootStorageName}";
            return $".{rootFolder}\\{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}";
        }
    }
}