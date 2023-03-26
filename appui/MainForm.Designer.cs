namespace appui
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ubt_refresh = new System.Windows.Forms.Button();
            ulv_clients = new System.Windows.Forms.CheckedListBox();
            upb_progress = new System.Windows.Forms.ProgressBar();
            utx_sqlquery = new System.Windows.Forms.TextBox();
            ucb_branch = new System.Windows.Forms.ComboBox();
            ubt_run = new System.Windows.Forms.Button();
            utx_search = new System.Windows.Forms.TextBox();
            utx_clientname = new System.Windows.Forms.TextBox();
            utx_servername = new System.Windows.Forms.TextBox();
            utx_outputpath = new System.Windows.Forms.TextBox();
            utx_dbname = new System.Windows.Forms.TextBox();
            lblProgress = new System.Windows.Forms.Label();
            btn_selectall = new System.Windows.Forms.Button();
            btn_openJsonConfig = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // ubt_refresh
            // 
            ubt_refresh.Location = new System.Drawing.Point(176, 6);
            ubt_refresh.Margin = new System.Windows.Forms.Padding(2);
            ubt_refresh.Name = "ubt_refresh";
            ubt_refresh.Size = new System.Drawing.Size(119, 36);
            ubt_refresh.TabIndex = 0;
            ubt_refresh.Text = "refresh";
            ubt_refresh.UseVisualStyleBackColor = true;
            ubt_refresh.Click += button_refresh_Click;
            // 
            // ulv_clients
            // 
            ulv_clients.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ulv_clients.FormattingEnabled = true;
            ulv_clients.Location = new System.Drawing.Point(731, 42);
            ulv_clients.Margin = new System.Windows.Forms.Padding(2);
            ulv_clients.Name = "ulv_clients";
            ulv_clients.Size = new System.Drawing.Size(191, 328);
            ulv_clients.Sorted = true;
            ulv_clients.TabIndex = 4;
            ulv_clients.UseTabStops = false;
            ulv_clients.SelectedIndexChanged += ulv_clients_SelectedIndexChanged;
            // 
            // upb_progress
            // 
            upb_progress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            upb_progress.Location = new System.Drawing.Point(7, 475);
            upb_progress.Name = "upb_progress";
            upb_progress.Size = new System.Drawing.Size(915, 23);
            upb_progress.TabIndex = 6;
            // 
            // utx_sqlquery
            // 
            utx_sqlquery.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            utx_sqlquery.Enabled = false;
            utx_sqlquery.Location = new System.Drawing.Point(12, 48);
            utx_sqlquery.Multiline = true;
            utx_sqlquery.Name = "utx_sqlquery";
            utx_sqlquery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            utx_sqlquery.Size = new System.Drawing.Size(719, 391);
            utx_sqlquery.TabIndex = 2;
            utx_sqlquery.Text = "select max(featureid) as featureid from feature with(nolock)";
            // 
            // ucb_branch
            // 
            ucb_branch.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ucb_branch.FormattingEnabled = true;
            ucb_branch.ItemHeight = 28;
            ucb_branch.Location = new System.Drawing.Point(6, 6);
            ucb_branch.Name = "ucb_branch";
            ucb_branch.Size = new System.Drawing.Size(165, 36);
            ucb_branch.TabIndex = 1;
            ucb_branch.SelectedIndexChanged += ucb_branch_SelectedIndexChanged;
            // 
            // ubt_run
            // 
            ubt_run.Enabled = false;
            ubt_run.Location = new System.Drawing.Point(408, 6);
            ubt_run.Name = "ubt_run";
            ubt_run.Size = new System.Drawing.Size(137, 36);
            ubt_run.TabIndex = 3;
            ubt_run.Text = "run";
            ubt_run.UseVisualStyleBackColor = true;
            ubt_run.Click += ubt_run_Click;
            // 
            // utx_search
            // 
            utx_search.Enabled = false;
            utx_search.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            utx_search.Location = new System.Drawing.Point(562, 8);
            utx_search.Name = "utx_search";
            utx_search.PlaceholderText = "search";
            utx_search.Size = new System.Drawing.Size(164, 34);
            utx_search.TabIndex = 7;
            utx_search.TextChanged += utb_search_TextChanged;
            // 
            // utx_clientname
            // 
            utx_clientname.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            utx_clientname.Location = new System.Drawing.Point(733, 428);
            utx_clientname.Name = "utx_clientname";
            utx_clientname.ReadOnly = true;
            utx_clientname.Size = new System.Drawing.Size(189, 23);
            utx_clientname.TabIndex = 8;
            // 
            // utx_servername
            // 
            utx_servername.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            utx_servername.Location = new System.Drawing.Point(730, 452);
            utx_servername.Name = "utx_servername";
            utx_servername.ReadOnly = true;
            utx_servername.Size = new System.Drawing.Size(191, 23);
            utx_servername.TabIndex = 9;
            // 
            // utx_outputpath
            // 
            utx_outputpath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            utx_outputpath.Cursor = System.Windows.Forms.Cursors.Hand;
            utx_outputpath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            utx_outputpath.Location = new System.Drawing.Point(6, 452);
            utx_outputpath.Margin = new System.Windows.Forms.Padding(2);
            utx_outputpath.Name = "utx_outputpath";
            utx_outputpath.ReadOnly = true;
            utx_outputpath.Size = new System.Drawing.Size(719, 23);
            utx_outputpath.TabIndex = 10;
            utx_outputpath.TabStop = false;
            utx_outputpath.MouseClick += utx_outputpath_MouseClick;
            // 
            // utx_dbname
            // 
            utx_dbname.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            utx_dbname.Location = new System.Drawing.Point(733, 404);
            utx_dbname.Name = "utx_dbname";
            utx_dbname.ReadOnly = true;
            utx_dbname.Size = new System.Drawing.Size(189, 23);
            utx_dbname.TabIndex = 8;
            // 
            // lblProgress
            // 
            lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblProgress.AutoSize = true;
            lblProgress.BackColor = System.Drawing.Color.Transparent;
            lblProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblProgress.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblProgress.Location = new System.Drawing.Point(7, 475);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new System.Drawing.Size(0, 21);
            lblProgress.TabIndex = 11;
            // 
            // btn_selectall
            // 
            btn_selectall.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            btn_selectall.Enabled = false;
            btn_selectall.Location = new System.Drawing.Point(730, 12);
            btn_selectall.Name = "btn_selectall";
            btn_selectall.Size = new System.Drawing.Size(75, 23);
            btn_selectall.TabIndex = 12;
            btn_selectall.Tag = "";
            btn_selectall.Text = "select all";
            btn_selectall.UseVisualStyleBackColor = true;
            btn_selectall.Click += button_selectall_Click;
            // 
            // btn_openJsonConfig
            // 
            btn_openJsonConfig.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btn_openJsonConfig.Location = new System.Drawing.Point(863, 375);
            btn_openJsonConfig.Name = "btn_openJsonConfig";
            btn_openJsonConfig.Size = new System.Drawing.Size(58, 23);
            btn_openJsonConfig.TabIndex = 13;
            btn_openJsonConfig.Text = "JSON";
            btn_openJsonConfig.UseVisualStyleBackColor = true;
            btn_openJsonConfig.Click += btn_openJsonConfig_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(924, 510);
            Controls.Add(btn_openJsonConfig);
            Controls.Add(btn_selectall);
            Controls.Add(lblProgress);
            Controls.Add(utx_dbname);
            Controls.Add(utx_outputpath);
            Controls.Add(utx_servername);
            Controls.Add(utx_clientname);
            Controls.Add(utx_search);
            Controls.Add(ubt_run);
            Controls.Add(ucb_branch);
            Controls.Add(utx_sqlquery);
            Controls.Add(upb_progress);
            Controls.Add(ulv_clients);
            Controls.Add(ubt_refresh);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(2);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button ubt_refresh;
        private System.Windows.Forms.CheckedListBox ulv_clients;
        private System.Windows.Forms.ProgressBar upb_progress;
        private System.Windows.Forms.TextBox utx_sqlquery;
        private System.Windows.Forms.ComboBox ucb_branch;
        private System.Windows.Forms.Button ubt_run;
        private System.Windows.Forms.TextBox utx_search;
        private System.Windows.Forms.TextBox utx_clientname;
        private System.Windows.Forms.TextBox utx_servername;
        private System.Windows.Forms.TextBox utx_outputpath;
        private System.Windows.Forms.TextBox utx_dbname;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btn_selectall;
        private System.Windows.Forms.Button btn_openJsonConfig;
    }
}

