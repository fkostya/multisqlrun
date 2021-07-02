using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace appui
{
    public partial class Form1 : Form
    {
        private bool offline = Config.Offline;
        private Dictionary<string, List<PageRow>> parsedDoc;
        private bool openConnectionProcess;
        
        public Form1()
        {
            InitializeComponent();
        }

        private async void ubt_connect_Click(object sender, EventArgs e)
        {
            try
            {
                upb_progress.Style = ProgressBarStyle.Marquee;
                upb_progress.MarqueeAnimationSpeed = 30;

                utx_search.Enabled = false;
                utx_sqlquery.Enabled = false;
                ucb_branch.Enabled = false;
                ubt_run.Enabled = false;

                clear();

                var doc = await this.loadHtml(offline ? Config.OfflineFilePath : Config.Url);
                this.parsedDoc = PageParser.Parse(doc);

                refresh(this.parsedDoc);

                openConnectionProcess = false;

                ucb_branch.SelectedIndex = 0;

                utx_search.Enabled = true;
                utx_sqlquery.Enabled = true;
                ucb_branch.Enabled = true;
                ubt_run.Enabled = true;

                upb_progress.Style = ProgressBarStyle.Blocks;
                upb_progress.MarqueeAnimationSpeed = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clear()
        {
            ucb_branch.Items.Clear();
            ulv_clients.Items.Clear();
        }

        private void refresh(Dictionary<string, List<PageRow>> parsed)
        {
            var index = 0;

            parsed.Keys.ToList().OrderByDescending(k => k).ToList().ForEach(k => ucb_branch.Items.Insert(index++, k));

            updateClientList();
        }

        private async Task<HtmlDocument> loadHtml(string url)
        {
            this.Text = url;

            IPageReader loader = PageReaderFactory.CreatePageReader(offline);

            return await PageReader.GetPageAsync(loader, url);
        }

        private async void ubt_run_Click(object sender, EventArgs e)
        {
            if (offline)
            {
                return;
            }

            ubt_run.Enabled = false;
            utx_outputpath.Text = "";

            Dictionary<string, List<Tuple<string, string>>> result = new Dictionary<string, List<Tuple<string, string>>>();

            if (!string.IsNullOrWhiteSpace(utx_sqlquery.Text))
            {
                var processed = 0;
                int? totalToProcess = 0;
                try
                {
                    var clients = getAllClientsOrSelected(parsedDoc, ucb_branch.SelectedItem?.ToString());

                    var connections = new List<Tuple<SqlConnectionStringBuilder, CancellationTokenSource>>();
                    foreach (var singleClient in clients)
                    {
                        connections.Add(new Tuple<SqlConnectionStringBuilder, CancellationTokenSource>(
                            new SqlConnectionStringBuilder()
                            {
                                DataSource = singleClient.server,
                                InitialCatalog = singleClient.database,
                                UserID = Config.SqlUname,
                                Password = Config.SqlPwd,
                                ConnectTimeout = Config.TimeputOpenConnection
                            },
                            new CancellationTokenSource()
                        ));
                    }

                    var current = 0;
                    totalToProcess = clients?.Count();

                    foreach (var clientConnection in connections)
                    {
                        Interlocked.Increment(ref current);

                        lblProgress.Text = $"{current} of {totalToProcess}";

                        var source = clientConnection.Item2;
                        initDbConnectionProcess(source.Token);

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(clientConnection.Item1.ConnectionString))
                            {
                                await connection.OpenAsync();

                                updateClientProgress(clients.Count, current);

                                var output = await runQueryAsync(connection);
                                    
                                lock (this)
                                {
                                    result[connection.Database] = output;
                                    Interlocked.Increment(ref processed);
                                }
                            }
                        }
                        catch
                        {
                            stopDbConnectionProcess(source);
                        }
                        finally
                        {
                            updateClientProgress(clients.Count, current);
                        }
                    }
                }
                catch (Exception ex)
                {
                    stopDbConnectionProcess(null);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    stopDbConnectionProcess(null);

                    //result = getFakeOutput();

                    var path = "error";
                    try
                    {
                        var waitingTime = 0;
                        while(result.Count != totalToProcess && waitingTime <= Config.StopAfterMilliseconds)
                        {
                            waitingTime += 1000;
                            await Task.Delay(1000);
                            updateClientProgress(1, 1);
                        }

                        if(waitingTime == Config.StopAfterMilliseconds)
                        {
                            MessageBox.Show(string.Format("processed {0} out of total {1}", result.Count, totalToProcess));
                        }

                        path = await saveOutputToCsv(result);

                        upb_progress.Value = upb_progress.Maximum;
                    }
                    catch { }

                    utx_outputpath.Text = path;
                    ubt_run.Enabled = true;
                }
            }
        }

        private List<PageRow> getAllClientsOrSelected(Dictionary<string, List<PageRow>> parsedDoc, string branch)
        {
            if (this.parsedDoc.ContainsKey(branch))
            {
                var clients = parsedDoc[branch]
                                .Where(f => ulv_clients.Items.Cast<string>().Any(f1 => f1 == f.client))
                                .ToList();

                if (ulv_clients.SelectedItems.Count != 0)
                {
                    var selected = ulv_clients.SelectedItems?.Cast<string>().ToList();

                    clients = clients.Where(f => selected.Any(f1 => f1 == f.client)).ToList();
                }

            return clients;
            }

            return new List<PageRow>();
        }

        private Dictionary<string, List<Tuple<string, string>>> getFakeOutput()
        {
            var output = new Dictionary<string, List<Tuple<string, string>>>();

            output["client1"] = new List<Tuple<string, string>>{
                new Tuple<string, string>("field1", "1"),
                new Tuple<string, string>("field2", "11"),
            };

            output["client2"] = new List<Tuple<string, string>>{
                new Tuple<string, string>("field1", "2"),
                new Tuple<string, string>("field2", "22"),
            };

            return output;
        }

        private void updateClientProgress(int max, int current)
        {
            upb_progress.Maximum = max + 1;
            upb_progress.Value = current;
        }

        private void initDbConnectionProcess(CancellationToken token)
        {
            upb_progress.Maximum = Config.TimeputOpenConnection;

            var progress = new Progress<int>(value => upb_progress.Value = Math.Max(0, Math.Min(upb_progress.Maximum, value)));
            openConnectionProcess = true;

            Task.Run(() =>
            {
                var i = 0;

                while (openConnectionProcess)
                {
                    ((IProgress<int>)progress).Report(i++);
                    Thread.Sleep(100);

                    if (i == Config.TimeputOpenConnection)
                        i = 0;
                }
            }, token);
        }

        private void stopDbConnectionProcess(CancellationTokenSource source)
        {
            upb_progress.Value = 0;
            openConnectionProcess = false;

            if (source != null)
            {
                source.Cancel();
            }
        }

        public string path { get; set; }

        private void createFolderName()
        {
            var uniqueFolderName = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}";

            this.path = $".\\output\\{uniqueFolderName}";

            new DirectoryInfo(path).Create();
        }

        private async Task<string> saveOutputToCsv(Dictionary<string, List<Tuple<string, string>>> output)
        {
            try
            {
                createFolderName();

                var records = new List<dynamic>();

                foreach (var item in output)
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

                using (var writer = new StreamWriter($"{this.path}\\output.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(records);
                }

                return await Task.FromResult(Path.GetFullPath(path));
            }
            catch
            {
            }

            return null;
        }

        private async Task<List<Tuple<string, string>>> runQueryAsync(SqlConnection connection)
        {
            var output = new List<Tuple<string, string>>();
            try
            {

                var reader = await runCommandAsync(utx_sqlquery.Text, connection);

                try
                {
                    DataTable schemaTable = reader.GetSchemaTable();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                foreach (DataColumn column in schemaTable.Columns)
                                {
                                    var colname = row[column].ToString();

                                    output.Add(new Tuple<string, string>(colname, reader[colname].ToString()));

                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            catch
            {
            }

            return await Task.FromResult(output);
        }


        private async Task<SqlDataReader> runCommandAsync(string query, SqlConnection dbc)
        {
            using (SqlCommand cmd = new SqlCommand(query, dbc))
            {
                cmd.CommandTimeout = Config.TimeputOpenConnection;

                return await cmd.ExecuteReaderAsync();
            }
        }

        private void updateClientList()
        {
            ulv_clients.BeginUpdate();

            ulv_clients.Items.Clear();

            var branch = ucb_branch.SelectedItem?.ToString();

            if (branch!= null && parsedDoc.ContainsKey(branch))
            {
                showAllClients(parsedDoc[ucb_branch.SelectedItem.ToString()], utx_search.Text);
            }
            ulv_clients.EndUpdate();
        }

        private void ucb_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateClientList();
        }

        private void showAllClients(List<PageRow> clients, string filterName = "")
        {
            if (clients.Count > 0)
            {
                var index = 0;
                clients.ToList()
                    .Where(f => string.IsNullOrWhiteSpace(filterName) ||
                    f.client.Contains(filterName, StringComparison.OrdinalIgnoreCase) ||
                    f.database.Contains(filterName, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.client)
                        .ToList().ForEach(f =>
                        {
                            ulv_clients.Items.Insert(index++, f);
                        });
            }
        }
        private void utb_search_TextChanged(object sender, EventArgs e)
        {
            ulv_clients.BeginUpdate();

            ulv_clients.Items.Clear();
            _changeClientSection(null);

            showAllClients(this.parsedDoc[ucb_branch.SelectedItem.ToString()], utx_search.Text);

            ulv_clients.EndUpdate();
        }

        private void ulv_clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageRow client = null;

            if (!string.IsNullOrWhiteSpace(((ListBox)sender).SelectedItem?.ToString()))
            {
                client = this.parsedDoc[ucb_branch.SelectedItem.ToString()].FirstOrDefault(f => f.client.Equals(((ListBox)sender).SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            _changeClientSection(client);
        }

        private void _changeClientSection(PageRow client)
        {
            utx_dbname.Text = client?.database;
            utx_clientname.Text = client?.id;
            utx_servername.Text = client?.server;
        }

        private void utx_outputpath_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("explorer.exe", utx_outputpath.Text);
        }
    }
}