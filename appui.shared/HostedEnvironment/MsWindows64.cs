using appui.models.HostedEnvironment;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Text;

namespace appui.shared.HostedEnvironment
{
    public class MsWindows64 : IHostedEnvironment
    {
        private readonly ILogger<MsWindows64> logger;

        public MsWindows64(ILogger<MsWindows64> logger)
        {
            this.logger = logger;
        }

        public async Task Execute(object args)
        {
            if(args == null) { await Task.CompletedTask; return; }

            string fileName = (string)args;

            if (string.IsNullOrWhiteSpace(fileName)) { await Task.CompletedTask; return; }
            if (fileName.Length > 29) { await Task.CompletedTask; return; }

            using (IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application, null, null))
            {
                if (!store.FileExists(fileName))
                {
                    using (IsolatedStorageFileStream stream = new(fileName, FileMode.Create, FileAccess.Write, store))
                    {
                        var json = JsonConvert.SerializeObject(new ConfigSettingsJson()
                        {
                            MasterDbCredential = new Credential
                            {
                                UserName = "TODO: change username here",
                                Password = "TODO: change password here"
                            }
                        });

                        byte[] bytes = Encoding.ASCII.GetBytes(json);

                        await stream.WriteAsync(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public Task<ConfigSettingsJson> GetConfigFromSecureStorage(string fileName)
        {
            string fullPath = string.Empty;
            using (IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application, null, null))
            {
                using (IsolatedStorageFileStream stream = new(fileName, FileMode.Open, store))
                {
                    //fullPath = stream.GetType().GetField("_fullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(stream).ToString();

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var content = reader.ReadToEnd();

                        return Task.FromResult(JsonConvert.DeserializeObject<ConfigSettingsJson>(content));
                    }
                }
            }
        }

        public Task<string> GetConfigFullPath(string fileName) {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application, null, null))
            {
                using (IsolatedStorageFileStream stream = new(fileName, FileMode.Open, store))
                {
                    string fullPath = stream.GetType().GetField("_fullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(stream).ToString();

                    return Task.FromResult(fullPath);
                }
            }
        }
    }
}