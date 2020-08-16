using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
                var doc = await this.loadHtml(offline ? Config.OfflineFilePath : Config.Url);
                this.parsedDoc = PageParser.Parse(doc);

                refresh(this.parsedDoc);

                ucb_branch.SelectedIndex = 0;


                utx_search.Enabled = true;
                utx_sqlquery.Enabled = true;
                ucb_branch.Enabled = true;
                ubt_run.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refresh(Dictionary<string, List<PageRow>> parsed)
        {
            ucb_branch.Items.Clear();
            var index = 0;

            parsed.Keys.ToList().OrderByDescending(k => k).ToList().ForEach(k => ucb_branch.Items.Insert(index++, k));
        }
        private async Task<HtmlDocument> loadHtml(string url)
        {
            this.Text = url;

            IPageReader loader = null;
            if (offline == true)
            {
                loader = new OfflineFilePageReader();
            }
            else
            {
                loader = new WebPageReader();
            }

            return await PageReader.GetPageAsync(loader, url);
        }

        private async void ubt_run_Click(object sender, EventArgs e)
        {
            ubt_run.Enabled = false;
            utx_outputpath.Text = "";

            Dictionary<string, List<Tuple<string, string>>> result = new Dictionary<string, List<Tuple<string, string>>>();


            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token;

            if (!string.IsNullOrWhiteSpace(utx_sqlquery.Text))
            {
                try
                {
                    var branch = ucb_branch.SelectedItem?.ToString();

                    var clients = new List<PageRow>();

                    if (this.parsedDoc.ContainsKey(branch))
                    {
                        clients = parsedDoc[branch]
                            .Where(f => ulv_clients.Items.Cast<string>().Any(f1 => f1 == f.client))
                            .ToList(); ;

                        if (ulv_clients.SelectedItems.Count != 0)
                        {
                            var selected = ulv_clients.SelectedItems?.Cast<string>().ToList();

                            clients = clients.Where(f => selected.Any(f1 => f1 == f.client)).ToList();
                        }
                    }

                    var current = 0;

                    foreach (var client in clients?.GroupBy(f => f.server))
                    {
                        SqlConnectionStringBuilder sConnB = null;

                        foreach (var item in client)
                        {
                            current++;
                            bool changeDatabase = false;

                            if (sConnB == null)
                            {
                                sConnB = new SqlConnectionStringBuilder()
                                {
                                    DataSource = item.server,
                                    InitialCatalog = item.database,
                                    UserID = Config.SqlUname,
                                    Password = Config.SqlPwd,
                                    ConnectTimeout = Config.TimeputOpenConnection
                                };
                            }
                            else
                                changeDatabase = true;

                            token = source.Token;

                            Parallel.Invoke(() =>
                            {
                                initDbConnectionProcess(token);
                            });

                            using (SqlConnection connection = new SqlConnection(sConnB?.ConnectionString))
                            {
                                try
                                {
                                    if (changeDatabase)
                                        connection.ChangeDatabase(item.database);

                                    await connection.OpenAsync();
                                    stopDbConnectionProcess(source);

                                    updateClientProgress(clients.Count, current);

                                    var output = await runQueryAsync(connection);

                                    result[item.client] = output;
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
                        sConnB = null;
                    }
                }
                catch (Exception ex)
                {
                    stopDbConnectionProcess(source);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    stopDbConnectionProcess(source);

                    //result = getFakeOutput();

                    var path = "error";
                    try
                    {
                        path = await saveOutputToCsv(result);
                    }
                    catch { }

                    utx_outputpath.Text = path;
                    ubt_run.Enabled = true;
                }
            }
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

            var progress = new Progress<int>(value => upb_progress.Value = value);
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

        private async Task<string> saveOutputToCsv(Dictionary<string, List<Tuple<string, string>>> output)
        {
            try
            {
                var tick = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}";

                var path = $".\\output\\{tick}";

                new DirectoryInfo(path).Create();


                var records = new List<dynamic>();


                foreach (var item in output)
                {
                    dynamic record = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)record;
                    dictionary.Add("client", item.Key);

                    foreach (var item2 in item.Value)
                    {
                        dictionary.Add(item2.Item1, item2.Item2);
                    }
                    if (dictionary.Count > 1)
                        records.Add(dictionary);
                }

                using (var writer = new StreamWriter($"{path}\\output.csv"))
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

        private void ucb_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ulv_clients.BeginUpdate();

            ulv_clients.Items.Clear();

            var branch = ucb_branch.SelectedItem?.ToString();

            if (parsedDoc.ContainsKey(branch))
            {
                showAllClients(parsedDoc[ucb_branch.SelectedItem.ToString()], utx_search.Text);
            }
            ulv_clients.EndUpdate();
        }
        private void showAllClients(List<PageRow> clients, string filterName = "")
        {
            if (clients.Count > 0)
            {
                var index = 0;
                clients.ToList()
                    .Where(f => string.IsNullOrWhiteSpace(filterName) || f.client.Contains(filterName, StringComparison.OrdinalIgnoreCase))
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