namespace appui.shared
{
    public static class FileUtility
    {
        public static string GeneratePath
        {
            get
            {
                return $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}";
            }
        }
    }
}