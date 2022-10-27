namespace appui.shared.Utils
{
    public sealed class Optional
    {
        public static string Empty()
        {
            return string.Empty;
        }

        public static T Empty<T>() where T : class, new()
        {
            return new T();
        }
    }
}