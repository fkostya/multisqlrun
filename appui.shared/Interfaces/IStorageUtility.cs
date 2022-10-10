namespace appui.shared.Interfaces
{
    public interface IStorageUtility
    {
        DirectoryInfo CreateStorage(string path);

        string GenerateUniqueStorageName(string rootStorageName);
    }
}