namespace appui.shared.Interfaces
{
    public interface IStorageUtility
    {
        T CreateStorage<T>(string path);

        string GenerateUniqueStorageName(string rootStorageName);
    }
}