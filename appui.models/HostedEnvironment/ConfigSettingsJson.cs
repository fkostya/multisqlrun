namespace appui.models.HostedEnvironment
{
    public class Credential
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public struct ConfigSettingsJson
    {
        public Credential MasterDbCredential { get; set; }
    }
}