using appui.shared.Interfaces;
using appui.shared.Models;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class ProgressDbNotification : IProcressDbNotificaion
    {

    }

    public class SqlUtility : ISqlUtility
    {
        public IList<IConnectionStringInfo> Databases { get; set; }
        private IList<dynamic> output;
        private readonly AppSettings settings;
        private readonly ILogger logger;

        public SqlUtility(IOptions<AppSettings> settings, ILogger<AppErrorLog> logger)
        {
            this.settings = settings.Value;
            this.logger = logger;
        }

        public void Initialize(IList<IConnectionStringInfo> dbs)
        {
            Databases = dbs;
        }

        public void Run(string sqlquery, Action<IProcressDbNotificaion> progress)
        {
            foreach (var db in this.Databases)
            {

                progress(new ProgressDbNotification { Database = db.Database });
            }
        }

        public async Task<string> Save(string filepath)
        {
            try
            {
                string path = createFolderName();

                var records = new List<dynamic>();

                foreach (var item in this.output)
                {
                    dynamic record = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)record;
                    dictionary.Add("client", item.Key);

                    var column_index = 0;
                    foreach (var item2 in item.Value)
                    {
                        var uniqueColumnKey = item2.Item1;
                        if (dictionary.ContainsKey(item2.Item1))
                        {
                            uniqueColumnKey = $"{uniqueColumnKey}_{column_index++}";
                        }
                        dictionary[uniqueColumnKey] = item2.Item2;
                    }
                    if (dictionary.Count > 1)
                        records.Add(dictionary);
                }

                using (var writer = new StreamWriter($"{path}\\output.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(records);
                }

                return await Task.FromResult(Path.GetFullPath(this.settings.OutputFolder));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return null;
        }
        private string createFolderName()
        {
            try
            {
                string path = $"{this.settings.OutputFolder}{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}";
                new DirectoryInfo(path).Create();

                return path;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
