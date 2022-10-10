using appui.shared.Interfaces;

namespace appui.shared
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}