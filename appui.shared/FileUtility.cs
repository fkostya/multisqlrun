using appui.shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace appui.shared
{
    public class FileUtility : IStorageUtility
    {
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly ILogger _logger;

        public FileUtility(IDirectoryWrapper directoryWrapper, ILogger<FileUtility> logger)
        {
            this._directoryWrapper = directoryWrapper;
            this._logger = logger;
        }

        public DirectoryInfo CreateStorage(string path)
        {
            try
            {
                var directory = _directoryWrapper.CreateDirectory(path);
                this._logger.LogTrace($"Cretated directory: {directory}");
                return directory;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error: {ex}");
                throw;
            }
        }

        public string GenerateUniqueStorageName(string rootStorageName)
        {
            var rootFolder = string.IsNullOrEmpty(rootStorageName) ? string.Empty : $"\\{rootStorageName}";
            return $".{rootFolder}\\{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}";
        }
    }
}