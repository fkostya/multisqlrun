namespace appui
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ubt_connect = new System.Windows.Forms.Button();
            this.ulv_clients = new System.Windows.Forms.ListBox();
            this.upb_progress = new System.Windows.Forms.ProgressBar();
            this.utx_sqlquery = new System.Windows.Forms.TextBox();
            this.ucb_branch = new System.Windows.Forms.ComboBox();
            this.ubt_run = new System.Windows.Forms.Button();
            this.utx_search = new System.Windows.Forms.TextBox();
            this.utx_clientname = new System.Windows.Forms.TextBox();
            this.utx_servername = new System.Windows.Forms.TextBox();
            this.utx_outputpath = new System.Windows.Forms.TextBox();
            this.utx_dbname = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ubt_connect
            // 
            this.ubt_connect.Location = new System.Drawing.Point(7, 6);
            this.ubt_connect.Margin = new System.Windows.Forms.Padding(2);
            this.ubt_connect.Name = "ubt_connect";
            this.ubt_connect.Size = new System.Drawing.Size(183, 36);
            this.ubt_connect.TabIndex = 0;
            this.ubt_connect.Text = "connect";
            this.ubt_connect.UseVisualStyleBackColor = true;
            this.ubt_connect.Click += new System.EventHandler(this.ubt_connect_Click);
            // 
            // ulv_clients
            // 
            this.ulv_clients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulv_clients.FormattingEnabled = true;
            this.ulv_clients.ItemHeight = 15;
            this.ulv_clients.Location = new System.Drawing.Point(731, 6);
            this.ulv_clients.Margin = new System.Windows.Forms.Padding(2);
            this.ulv_clients.Name = "ulv_clients";
            this.ulv_clients.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ulv_clients.Size = new System.Drawing.Size(191, 394);
            this.ulv_clients.Sorted = true;
            this.ulv_clients.TabIndex = 4;
            this.ulv_clients.UseTabStops = false;
            this.ulv_clients.SelectedIndexChanged += new System.EventHandler(this.ulv_clients_SelectedIndexChanged);
            // 
            // upb_progress
            // 
            this.upb_progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.upb_progress.Location = new System.Drawing.Point(7, 475);
            this.upb_progress.Name = "upb_progress";
            this.upb_progress.Size = new System.Drawing.Size(915, 23);
            this.upb_progress.TabIndex = 6;
            // 
            // utx_sqlquery
            // 
            this.utx_sqlquery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.utx_sqlquery.Enabled = false;
            this.utx_sqlquery.Location = new System.Drawing.Point(7, 58);
            this.utx_sqlquery.Multiline = true;
            this.utx_sqlquery.Name = "utx_sqlquery";
            this.utx_sqlquery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.utx_sqlquery.Size = new System.Drawing.Size(719, 398);
            this.utx_sqlquery.TabIndex = 2;
            this.utx_sqlquery.Text = "select max(featureid) as featureid from feature";
            // 
            // ucb_branch
            // 
            this.ucb_branch.Enabled = false;
            this.ucb_branch.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ucb_branch.FormattingEnabled = true;
            this.ucb_branch.ItemHeight = 28;
            this.ucb_branch.Location = new System.Drawing.Point(196, 6);
            this.ucb_branch.Name = "ucb_branch";
            this.ucb_branch.Size = new System.Drawing.Size(122, 36);
            this.ucb_branch.TabIndex = 1;
            this.ucb_branch.SelectedIndexChanged += new System.EventHandler(this.ucb_branch_SelectedIndexChanged);
            // 
            // ubt_run
            // 
            this.ubt_run.Enabled = false;
            this.ubt_run.Location = new System.Drawing.Point(408, 6);
            this.ubt_run.Name = "ubt_run";
            this.ubt_run.Size = new System.Drawing.Size(137, 36);
            this.ubt_run.TabIndex = 3;
            this.ubt_run.Text = "run";
            this.ubt_run.UseVisualStyleBackColor = true;
            this.ubt_run.Click += new System.EventHandler(this.ubt_run_Click);
            // 
            // utx_search
            // 
            this.utx_search.Enabled = false;
            this.utx_search.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.utx_search.Location = new System.Drawing.Point(562, 8);
            this.utx_search.Name = "utx_search";
            this.utx_search.PlaceholderText = "search";
            this.utx_search.Size = new System.Drawing.Size(164, 34);
            this.utx_search.TabIndex = 7;
            this.utx_search.TextChanged += new System.EventHandler(this.utb_search_TextChanged);
            // 
            // utx_clientname
            // 
            this.utx_clientname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.utx_clientname.Location = new System.Drawing.Point(733, 428);
            this.utx_clientname.Name = "utx_clientname";
            this.utx_clientname.ReadOnly = true;
            this.utx_clientname.Size = new System.Drawing.Size(189, 23);
            this.utx_clientname.TabIndex = 8;
            // 
            // utx_servername
            // 
            this.utx_servername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.utx_servername.Location = new System.Drawing.Point(730, 452);
            this.utx_servername.Name = "utx_servername";
            this.utx_servername.ReadOnly = true;
            this.utx_servername.Size = new System.Drawing.Size(191, 23);
            this.utx_servername.TabIndex = 9;
            // 
            // utx_outputpath
            // 
            this.utx_outputpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.utx_outputpath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.utx_outputpath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.utx_outputpath.Location = new System.Drawing.Point(7, 453);
            this.utx_outputpath.Margin = new System.Windows.Forms.Padding(2);
            this.utx_outputpath.Name = "utx_outputpath";
            this.utx_outputpath.ReadOnly = true;
            this.utx_outputpath.Size = new System.Drawing.Size(719, 23);
            this.utx_outputpath.TabIndex = 10;
            this.utx_outputpath.TabStop = false;
            this.utx_outputpath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.utx_outputpath_MouseClick);
            // 
            // utx_dbname
            // 
            this.utx_dbname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.utx_dbname.Location = new System.Drawing.Point(733, 404);
            this.utx_dbname.Name = "utx_dbname";
            this.utx_dbname.ReadOnly = true;
            this.utx_dbname.Size = new System.Drawing.Size(189, 23);
            this.utx_dbname.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 510);
            this.Controls.Add(this.utx_dbname);
            this.Controls.Add(this.utx_outputpath);
            this.Controls.Add(this.utx_servername);
            this.Controls.Add(this.utx_clientname);
            this.Controls.Add(this.utx_search);
            this.Controls.Add(this.ubt_run);
            this.Controls.Add(this.ucb_branch);
            this.Controls.Add(this.utx_sqlquery);
            this.Controls.Add(this.upb_progress);
            this.Controls.Add(this.ulv_clients);
            this.Controls.Add(this.ubt_connect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ubt_connect;
        private System.Windows.Forms.ListBox ulv_clients;
        private System.Windows.Forms.ProgressBar upb_progress;
        private System.Windows.Forms.TextBox utx_sqlquery;
        private System.Windows.Forms.ComboBox ucb_branch;
        private System.Windows.Forms.Button ubt_run;
        private System.Windows.Forms.TextBox utx_search;
        private System.Windows.Forms.TextBox utx_clientname;
        private System.Windows.Forms.TextBox utx_servername;
        private System.Windows.Forms.TextBox utx_outputpath;
        private System.Windows.Forms.TextBox utx_dbname;
    }
}

