using System.Collections;

namespace appui.shared.Utils
{
    public static class OptionalOf
    {
        public static List<T> Of<T>(this IEnumerable listEx, params T[] @params)
        {
            return new List<T>(@params.Cast<T>().ToList());
        }
    }
}