using appui.models;
using appui.models.Interfaces;
using appui.models.Payloads;
using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        private string path = "";
        private async void ubt_run_Click(object sender, EventArgs e)
        {
            ubt_run.Enabled = false;
            utx_outputpath.Text = string.Empty;
            var query = utx_sqlquery.Text;
            
            if (!string.IsNullOrWhiteSpace(query))
            {
                int? totalToProcess = 0;
                try
                {
                    path = $".\\{appSetting.OutputFolder}\\{FileUtility.GeneratePath}";
                    new DirectoryInfo(path).Create();

                    var clients = getAllClientsOrSelected(ucb_branch.SelectedItem?.ToString());

                    var processed = 0;
                    foreach (var cc in clients)
                    {
                        lblProgress.Text = $"{processed} of {clients.Count()}";

                        var source = new CancellationTokenSource();
                        initDbConnectionProcess(source.Token);

                        try
                        {
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
                                payload.StoragePath = path;
                                this.Logger.LogTrace($"posting message to queue with payload:{payload}");
                                await this.messageProducer.Publish(payload);
                                this.Logger.LogTrace($"posted message to queue with payload:{payload}");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogCritical($"Exception: {ex}");
                            }

                            updateClientProgress(clients.Count, processed++);

                        }
                        catch
                        {
                            stopDbConnectionProcess(source);
                        }
                        finally
                        {
                            updateClientProgress(clients.Count, processed++);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger?.LogError($"Exception: {ex}");
                    stopDbConnectionProcess(null);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        stopDbConnectionProcess(null);
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