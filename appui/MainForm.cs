﻿using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using CsvHelper;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appui
{
    public partial class MainForm : Form
    {
        private IList<IConnectionRecord> parsedDoc;
        private bool openConnectionProcess;
        private readonly ILoadConnections connection;
        private readonly AppSettings config;
        private readonly ILogger logger;

        public MainForm(IOptions<AppSettings> options, ILoadConnections connection, ILogger<AppErrorLog> logger)
        {
            InitializeComponent();

            this.config = options.Value;
            this.connection = connection;
            this.logger = logger;

            logger.LogInformation($"start app {DateTime.Now}");
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
                btn_selectall.Enabled = false;

                clear();
                this.parsedDoc = await this.connection.Load();
                refresh();

                openConnectionProcess = false;

                ucb_branch.SelectedIndex = 0;

                utx_search.Enabled = true;
                utx_sqlquery.Enabled = true;
                ucb_branch.Enabled = true;
                ubt_run.Enabled = true;
                btn_selectall.Enabled = true;

                upb_progress.Style = ProgressBarStyle.Blocks;
                upb_progress.MarqueeAnimationSpeed = 0;

                if (this.config.Offline)
                {
                    ubt_run.Enabled = false;
                    utx_sqlquery.Enabled= false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                upb_progress.Value = 0;
                upb_progress.MarqueeAnimationSpeed = 0;
                upb_progress.Update();
            }
        }

        private void clear()
        {
            ucb_branch.Items.Clear();
            ulv_clients.Items.Clear();
            _changeClientSection(null);
        }

        private void refresh()
        {
            var index = 0;

            this.parsedDoc
                .DistinctBy(f => f.Version)
                .Select(f => f.Version)
                .ToList()
                .ForEach(f => ucb_branch.Items.Insert(index++, f));

            updateClientList();
        }

        private async void ubt_run_Click(object sender, EventArgs e)
        {
            ubt_run.Enabled = false;
            utx_outputpath.Text = "";

            Dictionary<string, List<Tuple<string, string>>> result = new Dictionary<string, List<Tuple<string, string>>>();

            var query  = utx_sqlquery.Text;
            if (!string.IsNullOrWhiteSpace(query))
            {
                var processed = 0;
                int? totalToProcess = 0;
                try
                {
                    var clients = getAllClientsOrSelected(ucb_branch.SelectedItem?.ToString());

                    var connections = new List<Tuple<SqlConnectionStringBuilder, CancellationTokenSource>>();
                    foreach (var singleClient in clients)
                    {
                        connections.Add(new Tuple<SqlConnectionStringBuilder, CancellationTokenSource>(
                            new SqlConnectionStringBuilder()
                            {
                                DataSource = singleClient.server,
                                InitialCatalog = singleClient.database,
                                UserID = this.config.DefaultSqlUserName,
                                Password = decode(this.config.DefaultSqlUserPwd),
                                ConnectTimeout = this.config.TimeputOpenConnection
                            },
                            new CancellationTokenSource()
                        ));
                    }

                    var current = 0;

                    foreach (var clientConnection in connections)
                    {
                        Interlocked.Increment(ref current);

                        lblProgress.Text = $"{current} of {clients?.Count()}";

                        var source = clientConnection.Item2;
                        initDbConnectionProcess(source.Token);

                        try
                        {
                            using (SqlRunCommand cmd = new SqlRunCommand(clientConnection.Item1))
                            {
                                updateClientProgress(clients.Count, current);

                                var output = await cmd.RunQuery(utx_sqlquery.Text);

                                lock (this)
                                {
                                    result[cmd.Database] = output;
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
                        while (result.Count != totalToProcess && waitingTime <= this.config.StopAfterMilliseconds)
                        {
                            waitingTime += 1000;
                            await Task.Delay(1000);
                            updateClientProgress(1, 1);
                        }

                        if (waitingTime == this.config.StopAfterMilliseconds)
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
     
        private string decode(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(data);
        }

        private IList<IConnectionRecord> getAllClientsOrSelected(string version)
        {
            var selected = new HashSet<string>(ulv_clients.SelectedItems.Count != 0
                ? ulv_clients.SelectedItems?.Cast<string>().ToList()
                : ulv_clients.Items.Cast<string>());

            return this.parsedDoc.Where(f => selected.Contains(f.key)).ToList();
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
            upb_progress.Maximum = config.TimeputOpenConnection;

            var progress = new Progress<int>(value => upb_progress.Value = Math.Max(0, Math.Min(upb_progress.Maximum, value)));
            openConnectionProcess = true;

            Task.Run(() =>
            {
                var i = 0;

                while (openConnectionProcess)
                {
                    ((IProgress<int>)progress).Report(i++);
                    Task.Delay(100);

                    if (i == config.TimeputOpenConnection)
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

        private void updateClientList()
        {
            ulv_clients.BeginUpdate();

            ulv_clients.Items.Clear();

            var version = ucb_branch.SelectedItem?.ToString();

            if (version != null)
            {
                showAllClients(parsedDoc.Where(f => f.Version == version).ToList(), utx_search.Text);
            }
            ulv_clients.EndUpdate();
        }

        private void ucb_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changeClientSection(null);
            updateClientList();
        }

        private void showAllClients(IList<IConnectionRecord> clients, string filterName = "")
        {
            if (clients.Count > 0)
            {
                var index = 0;
                clients.ToList()
                    .Where(f => string.IsNullOrWhiteSpace(filterName) ||
                    f.key.Contains(filterName, StringComparison.OrdinalIgnoreCase) ||
                    f.database.Contains(filterName, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.key)
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

            var version = ucb_branch.SelectedItem?.ToString();

            if (version != null)
            {
                showAllClients(parsedDoc.Where(f => f.Version == version).ToList(), utx_search.Text);
            }

            ulv_clients.EndUpdate();
        }

        private void ulv_clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = ((CheckedListBox)sender).SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var version = ucb_branch.SelectedItem?.ToString();
                var client = this.parsedDoc
                    .Where(f => f.Version == version && f.key == name)
                    .FirstOrDefault();

                _changeClientSection(client);
            }
        }

        private void _changeClientSection(IConnectionRecord client)
        {
            utx_dbname.Text = client?.database;
            utx_clientname.Text = client?.id;
            utx_servername.Text = client?.server;
        }

        private void utx_outputpath_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("explorer.exe", utx_outputpath.Text);
        }

        private void btn_selectall_Click(object sender, EventArgs e)
        {
            ulv_clients.BeginUpdate();
            if (ulv_clients.Items.Count != 0)
            {
                for (int i = 0; i < ulv_clients.Items.Count; i++)
                {
                    ulv_clients.SetItemChecked(i, true);
                }
            }
            ulv_clients.EndUpdate();
        }
    }
}