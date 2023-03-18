using appui.models;
using appui.models.HostedEnvironment;
using appui.models.Interfaces;
using appui.models.MsSql;
using appui.models.Payloads;
using appui.resourses.Properties;
using appui.shared.HostedEnvironment;
using appui.shared.Interfaces;
using appui.shared.Interfaces.Repositories;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private readonly IStorageUtility storeageUtility;
        private readonly IConnectorSettingsRepository connectorSettingsRepository;
        private readonly ConfigSettingsJson config;
        private readonly string configFullPath;

        private const int SelectedFirstItemIndex = 0;

        public MainForm(IOptions<AppSettings> appSettings,
            IOptions<SqlSettings> sqlSettings,
            ILogger<MainForm> logger,
            ITenantManager tenantManager,
            IServiceProvider serviceProvider,
            IMessageProducer messageProducer,
            IStorageUtility fileUtility,
            IConnectorSettingsRepository connectorSettingsRepository)
        {
            InitializeComponent();

            this.appSetting = appSettings.Value;
            this.sqlSettings = sqlSettings.Value;
            this.Logger = logger;
            this.tenantManager = tenantManager;
            this.serviceProvider = serviceProvider;
            this.messageProducer = messageProducer;
            this.storeageUtility = fileUtility;
            this.connectorSettingsRepository = connectorSettingsRepository;

            this.configFullPath = serviceProvider.GetService<IHostedEnvironment>().GetConfigFullPath(this.appSetting.JsonConfigFileName).Result;
            this.config = serviceProvider.GetService<IHostedEnvironment>().GetConfigFromSecureStorage(this.appSetting.JsonConfigFileName).Result;

            this.sqlSettings.Credential = new shared.Models.Credential
            {
                UserId = this.config.MasterDbCredential.UserName,
                Password = this.config.MasterDbCredential.Password
            };
            Logger.LogInformation($"App started");
        }

        private async void button_refresh_Click(object sender, EventArgs e)
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

                await refreshCatalogList();

                openConnectionProcess = false;
                ucb_branch.Enabled = true;

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

        private async Task refreshCatalogList()
        {
            var bindingComboBoxOriginalData = new BindingList<ConnectorSetting>((await connectorSettingsRepository.GetConnectorSettings()).ToList());

            ucb_branch.DataSource = bindingComboBoxOriginalData;
            ucb_branch.DisplayMember = "Name";
            ucb_branch.SelectedIndex = SelectedFirstItemIndex;
        }

        private async Task refreshTenantList()
        {
            ulv_clients.BeginUpdate();
            btn_selectall.Enabled = false;
            var tenants = (await tenantManager.LoadTenants((ConnectorSetting)ucb_branch.SelectedItem))
                            .Where((f) =>
                            {
                                return string.IsNullOrEmpty(utx_search.Text)
                                    || f.Name.Contains(utx_search.Text, StringComparison.OrdinalIgnoreCase)
                                    || f.Connection.Database.Contains(utx_search.Text, StringComparison.OrdinalIgnoreCase);
                            })
                            .ToList();


            var bindingComboBoxOriginalData = new BindingList<ITenant>(tenants);

            ulv_clients.DataSource = bindingComboBoxOriginalData;
            ulv_clients.DisplayMember = "Name";

            if (bindingComboBoxOriginalData.Any())
            {
                btn_selectall.Enabled = true;
                utx_search.Enabled = true;
                utx_sqlquery.Enabled = true;
                ubt_run.Enabled = true;
            }

            ulv_clients.EndUpdate();
        }


        private async void ubt_run_Click(object sender, EventArgs e)
        {
            ubt_run.Enabled = false;
            utx_outputpath.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(utx_sqlquery.Text))
            {
                try
                {
                    utx_outputpath.Text = storeageUtility.CreateStorage<DirectoryInfoWrapper>(storeageUtility.GenerateUniqueStorageName(appSetting.OutputFolder)).Info.FullName;
                    utx_outputpath.Enabled = false;

                    var tenants = getSelectedTenantsOrAllTenants();

                    var processed = 0;
                    foreach (var tenant in tenants)
                    {
                        lblProgress.Text = $"{processed} of {tenants.Count()}";
                        var source = new CancellationTokenSource();
                        initDbConnectionProcess(source.Token);

                        try
                        {
                            try
                            {
                                MsSqlMessagePayload payload = serviceProvider.GetService<MsSqlMessagePayload>();
                                payload.Connection = serviceProvider
                                    .GetService<Func<string, string, string, string, MsSqlConnection>>()
                                    .Invoke(tenant.Connection.Database,
                                            tenant.Connection.DbServer,
                                            tenant.Connection.UserName ?? sqlSettings.Credential.UserId,
                                            tenant.Connection.Password ?? decode(sqlSettings.Credential.Password));
                                payload.Query = utx_sqlquery.Text;
                                payload.StoragePath = utx_outputpath.Text;
                                this.Logger.LogTrace($"posting message to queue with payload:{payload}");
                                await this.messageProducer.Publish(payload);
                                this.Logger.LogTrace($"posted message to queue with payload:{payload}");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogCritical($"Exception: {ex}");
                            }
                        }
                        catch
                        {
                            stopDbConnectionProcess(source);
                        }
                        finally
                        {
                            updateClientProgress(tenants.Count, processed++);
                            lblProgress.Text = $"{processed} of {tenants.Count()}";
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

                    ubt_run.Enabled = true;
                    utx_outputpath.Enabled = true;
                }
            }
        }

        private string decode(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(data);
        }

        private IList<ITenant> getSelectedTenantsOrAllTenants()
        {
            return ulv_clients.CheckedItems.Count != 0
                ? ulv_clients.CheckedItems?.Cast<ITenant>().ToList()
                : ulv_clients.Items.Cast<ITenant>().ToList();
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

        private async void ucb_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            await refreshTenantList();
            _showSelectedTenantInfo(null);
        }

        private async void utb_search_TextChanged(object sender, EventArgs e)
        {
            await refreshTenantList();
        }

        private void ulv_clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            _showSelectedTenantInfo(((CheckedListBox)sender).SelectedItem as ITenant);
        }

        private void _showSelectedTenantInfo(ITenant tenant)
        {
            utx_dbname.Text = tenant?.Connection?.Database;
            utx_clientname.Text = tenant?.Name;
            utx_servername.Text = tenant?.Connection.DbServer;
        }

        private void utx_outputpath_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start(appSetting.Explorer, utx_outputpath.Text);
        }

        private void button_selectall_Click(object sender, EventArgs e)
        {
            var currentState = string.IsNullOrEmpty(btn_selectall.Tag.ToString()) || !(bool)btn_selectall.Tag;

            btn_selectall.Text = currentState ? Resources.lblunselectall : Resources.lblselectall;
            btn_selectall.Tag = currentState;

            ulv_clients.BeginUpdate();
            if (ulv_clients.Items.Count != 0)
            {
                for (int i = 0; i < ulv_clients.Items.Count; i++)
                {
                    ulv_clients.SetItemChecked(i, currentState);
                }
            }
            ulv_clients.EndUpdate();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await refreshCatalogList();
        }

        private void btn_openJsonConfig_Click(object sender, EventArgs e)
        {
            Process.Start(appSetting.Explorer, configFullPath);
        }
    }
}