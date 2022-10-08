using appui.connectors;
using appui.models;
using appui.models.Interfaces;
using appui.models.Payloads;
using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private IList<IConnectionStringInfo> parsedDoc;
        private bool openConnectionProcess;
        private readonly AppSettings appSetting;
        private readonly SqlSettings sqlSettings;
        private readonly ILogger<MainForm> Logger;
        private readonly ITenantManager tenantManager;
        private readonly IServiceProvider serviceProvider;
        private readonly IMessageProducer messageProducer;

        public MainForm(IOptions<AppSettings> appSettings,
            IOptions<SqlSettings> sqlSettings,
            ILogger<MainForm> logger,
            ITenantManager tenantManager,
            IServiceProvider serviceProvider,
            IMessageProducer messageProducer)
        {
            InitializeComponent();

            this.appSetting = appSettings.Value;
            this.sqlSettings = sqlSettings.Value;
            this.Logger = logger;
            this.tenantManager = tenantManager;
            this.serviceProvider = serviceProvider;
            this.messageProducer = messageProducer;

            Logger.LogInformation($"App started");

            //messageProducer.Publish()
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
                await refresh();

                openConnectionProcess = false;

                ucb_branch.SelectedIndex = 0;

                utx_search.Enabled = true;
                utx_sqlquery.Enabled = true;
                ucb_branch.Enabled = true;
                ubt_run.Enabled = true;
                btn_selectall.Enabled = true;

                upb_progress.Style = ProgressBarStyle.Blocks;
                upb_progress.MarqueeAnimationSpeed = 0;

                //if (this.defaultConnector.Offline)
                //{
                //    ubt_run.Enabled = false;
                //    utx_sqlquery.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, ex.Message);
                upb_progress.Style = ProgressBarStyle.Blocks;
                updateClientProgress(0, 0);
                MessageBox.Show(ex.Message);
            }
        }

        private void clear()
        {
            ucb_branch.Items.Clear();
            ulv_clients.Items.Clear();
            _changeClientSection(null);
        }

        private async Task<int> refresh()
        {
            var connections = await tenantManager.LoadTenants(null);

            connections
                .DistinctBy(f => f.Version)
                .Select((f, i) => new { f.Version, i })
                .ToList()
                .ForEach(f => ucb_branch.Items.Insert(f.i, f.Version));

            ucb_branch.SelectedIndex = 0;

            updateListOfClients();

            return await Task.FromResult(0);
        }

        private async void ubt_run_Click(object sender, EventArgs e)
        {
            ubt_run.Enabled = false;
            utx_outputpath.Text = "";

            Dictionary<string, List<Tuple<string, string>>> result = new Dictionary<string, List<Tuple<string, string>>>();

            var query = utx_sqlquery.Text;
            if (!string.IsNullOrWhiteSpace(query))
            {
                var processed = 0;
                int? totalToProcess = 0;
                try
                {
                    var clients = getAllClientsOrSelected(ucb_branch.SelectedItem?.ToString());

                    //new SqlRunCommand().Run(utx_sqlquery.Text, clients, (current) => { updateClientProgress(clients.Count, current); });

                    var current = 0;
                    createFolderName();

                    foreach (var cc in clients)
                    {
                        Interlocked.Increment(ref current);

                        lblProgress.Text = $"{current} of {clients?.Count()}";

                        var source = new CancellationTokenSource();
                        initDbConnectionProcess(source.Token);

                        try
                        {
                            List<Dictionary<string, object>> output = new List<Dictionary<string, object>>();
                            try
                            {
                                MsSqlMessagePayload payload = (MsSqlMessagePayload)serviceProvider.GetService<IMessagePayload>();
                                payload.Connection = new MsSqlConnection
                                {
                                    DbDatabase = cc.Connection.Database,
                                    DbServer = cc.Connection.DbServer,
                                    DbUserName = sqlSettings.Credential.UserId,
                                    DbPassword = decode(sqlSettings.Credential.Password)
                                };
                                payload.Query = utx_sqlquery.Text;
                                this.Logger.LogTrace($"posting message to queue with payload:{payload}");
                                this.messageProducer.Publish(payload);
                                this.Logger.LogTrace($"posted message to queue with payload:{payload}");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogCritical($"Exception: {ex}");
                            }

                            updateClientProgress(clients.Count, current);

                            lock (this)
                            {
                                result[cc.Connection.Database] = (List<Tuple<string, string>>)output
                                    .Select(f =>
                                    {
                                        var list = new List<Tuple<string, string>>();
                                        foreach (var k in f.Keys)
                                        {
                                            if (f[k] != null)
                                            {
                                                list.Add(new Tuple<string, string>(k, f[k].ToString()));
                                            }
                                        }
                                        return list;
                                    });
                                Interlocked.Increment(ref processed);
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
                        while (result.Count != totalToProcess && waitingTime <= this.appSetting.StopAfterMilliseconds)
                        {
                            waitingTime += 1000;
                            await Task.Delay(1000);
                            updateClientProgress(1, 1);
                        }

                        if (waitingTime == this.appSetting.StopAfterMilliseconds)
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

        private IList<ITenant> getAllClientsOrSelected(string version)
        {
            var selected = new HashSet<string>(ulv_clients.SelectedItems.Count != 0
                ? ulv_clients.SelectedItems?.Cast<string>().ToList()
                : ulv_clients.Items.Cast<string>());

            return this.tenantManager.LoadTenants(null).Result
                .ToList()
                .Where(f => selected.Contains(f.Name))
                .ToList();
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

        private async void initDbConnectionProcess(CancellationToken token)
        {
            upb_progress.Maximum = sqlSettings.ConnectionTimeout;

            var progress = new Progress<int>(value => upb_progress.Value = Math.Max(0, Math.Min(upb_progress.Maximum, value)));
            openConnectionProcess = true;

            await Task.Run(() =>
            {
                var i = 0;

                while (openConnectionProcess)
                {
                    ((IProgress<int>)progress).Report(i++);
                    Task.Delay(100);

                    if (i == sqlSettings.ConnectionTimeout)
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

            this.path = $".\\{appSetting.OutputFolder}\\{uniqueFolderName}";

            new DirectoryInfo(path).Create();
        }

        private async Task<string> saveOutputToCsv(Dictionary<string, List<Tuple<string, string>>> output)
        {
            try
            {
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

        private void updateListOfClients()
        {
            ulv_clients.BeginUpdate();
            ulv_clients.Items.Clear();

            tenantManager
               .FindTenants(ucb_branch.SelectedItem?.ToString(), utx_search.Text)
               .Select((item, index) => new { item = item, index = index })
               .ToList()
               .ForEach(f =>
               {
                   ulv_clients.Items.Insert(f.index, f.item.Name);
               });

            ulv_clients.EndUpdate();
        }

        private void ucb_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changeClientSection(null);
            updateListOfClients();
        }

        private void utb_search_TextChanged(object sender, EventArgs e)
        {
            ulv_clients.BeginUpdate();

            ulv_clients.Items.Clear();
            _changeClientSection(null);

            tenantManager
                .FindTenants(ucb_branch.SelectedItem?.ToString(), utx_search.Text)
                .Select((item, index) => new { item = item, index = index })
                .ToList()
                .ForEach(f =>
                {
                    ulv_clients.Items.Insert(f.index, f.item.Name);
                });

            ulv_clients.EndUpdate();
        }

        private void ulv_clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tenant = tenantManager
                .FindTenants(ucb_branch.SelectedItem?.ToString(), ((CheckedListBox)sender).SelectedItem?.ToString())
                .Where(f => f.Name == ((CheckedListBox)sender).SelectedItem?.ToString())
                .FirstOrDefault();

            _changeClientSection(tenant);
        }

        private void _changeClientSection(ITenant tenant)
        {
            utx_dbname.Text = tenant?.Connection?.Database;
            utx_clientname.Text = tenant?.Connection?.UserName;
            utx_servername.Text = tenant?.Connection.DbServer;
        }

        private void utx_outputpath_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start(appSetting.Explorer, utx_outputpath.Text);
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