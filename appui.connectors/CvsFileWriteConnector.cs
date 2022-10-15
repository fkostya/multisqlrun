using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

namespace appui.connectors
{
    public class CvsFileWriteConnector
    {
        private readonly ILogger _logger;

        public CvsFileWriteConnector(ILogger<CvsFileWriteConnector> logger)
        {
            this._logger = logger;
        }

        public async Task Invoke(List<Dictionary<string, object>> writeRecords, string path)
        {
            try
            {
                this._logger.Log(LogLevel.Information, $"writing #{writeRecords.Count} to cvs file: ${path}");
                var records = new List<dynamic>();

                foreach (var item in writeRecords)
                {
                    dynamic record = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)record;
                    dictionary.Add("client", "item.Key");//TODO

                    //var column_index = 0;
                    foreach (var item2 in item)
                    {
                        //var uniqueColumnKey = item2.Item1;
                        //if (dictionary.ContainsKey(item2.Item1))
                        //{
                        //    uniqueColumnKey = $"{uniqueColumnKey}_{column_index++}";
                        //}
                        //dictionary[uniqueColumnKey] = item2.Item2;
                    }
                    if (dictionary.Count > 1)
                        records.Add(dictionary);
                }

                using (var writer = new StreamWriter($"{path}\\output.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(records);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erorr: {ex}");
            }
        }
    }
}