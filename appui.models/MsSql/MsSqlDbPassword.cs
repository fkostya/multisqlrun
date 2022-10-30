using System.Diagnostics.CodeAnalysis;

namespace appui.models.MsSql
{
    [ExcludeFromCodeCoverage]
    public class MsSqlDbPassword
    {
        public string Password { get; }

        public MsSqlDbPassword(string password)
        {
            Password = password;
        }
    }
}