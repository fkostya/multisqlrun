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

        public static string FilePath(string path)
        {
            var env_variable = '$';
            var path_splitter = "\\";
            if (!string.IsNullOrEmpty(path))
            {
                var path_split = path.Split(path_splitter);
                var new_path = new List<string>();
                for (var i = 0; i < path_split.Length; i++)
                {
                    new_path.Add(path_split[i][0] == env_variable ?
                        Environment.GetEnvironmentVariable(path_split[i].TrimStart('$')).TrimStart('\\').TrimStart('\\')
                        : path_split[i]);
                }
                return new_path.Aggregate((a, b) => a + path_splitter + b);
            }
            return path;
        }
    }
}