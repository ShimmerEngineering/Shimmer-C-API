namespace ShimmerAPI
{
    partial class FormUpdateInstall
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkbxOverwriteFW = new System.Windows.Forms.CheckBox();
            this.dataGridViewUpdates = new System.Windows.Forms.DataGridView();
            this.backgroundWorkerUpdatesInstall = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(350, 188);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(77, 23);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(449, 188);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkbxOverwriteFW
            // 
            this.chkbxOverwriteFW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxOverwriteFW.AutoSize = true;
            this.chkbxOverwriteFW.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkbxOverwriteFW.Checked = true;
            this.chkbxOverwriteFW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbxOverwriteFW.Enabled = false;
            this.chkbxOverwriteFW.Location = new System.Drawing.Point(177, 191);
            this.chkbxOverwriteFW.Name = "chkbxOverwriteFW";
            this.chkbxOverwriteFW.Size = new System.Drawing.Size(157, 17);
            this.chkbxOverwriteFW.TabIndex = 2;
            this.chkbxOverwriteFW.Text = "Overwrite old Bootstrap files";
            this.chkbxOverwriteFW.UseVisualStyleBackColor = true;
            // 
            // dataGridViewUpdates
            // 
            this.dataGridViewUpdates.AllowUserToAddRows = false;
            this.dataGridViewUpdates.AllowUserToDeleteRows = false;
            this.dataGridViewUpdates.AllowUserToResizeColumns = false;
            this.dataGridViewUpdates.AllowUserToResizeRows = false;
            this.dataGridViewUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewUpdates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUpdates.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewUpdates.MultiSelect = false;
            this.dataGridViewUpdates.Name = "dataGridViewUpdates";
            this.dataGridViewUpdates.RowHeadersVisible = false;
            this.dataGridViewUpdates.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewUpdates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUpdates.Size = new System.Drawing.Size(512, 170);
            this.dataGridViewUpdates.TabIndex = 3;
            // 
            // FormUpdateInstall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 223);
            this.Controls.Add(this.dataGridViewUpdates);
            this.Controls.Add(this.chkbxOverwriteFW);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDownload);
            this.Icon = global::ShimmerAPI.Properties.Resources.icon;
            this.Name = "FormUpdateInstall";
            this.Text = "Updates";
            this.Load += new System.EventHandler(this.FormUpdateInstall_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox chkbxOverwriteFW;
        private System.Windows.Forms.DataGridView dataGridViewUpdates;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdatesInstall;
    }
}