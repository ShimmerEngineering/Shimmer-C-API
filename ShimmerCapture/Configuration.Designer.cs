namespace ShimmerAPI
{
    partial class Configuration
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlGeneralConfig1 = new ShimmerAPI.UserControlGeneralConfig();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlExgConfig1 = new ShimmerAPI.UserControlExgConfig();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.userControlSdConfig1 = new ShimmerAPI.UserControlSdConfig();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(16, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1041, 575);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlGeneralConfig1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1033, 546);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userControlGeneralConfig1
            // 
            this.userControlGeneralConfig1.Location = new System.Drawing.Point(8, 7);
            this.userControlGeneralConfig1.Margin = new System.Windows.Forms.Padding(5);
            this.userControlGeneralConfig1.Name = "userControlGeneralConfig1";
            this.userControlGeneralConfig1.Size = new System.Drawing.Size(1019, 542);
            this.userControlGeneralConfig1.TabIndex = 0;
            this.userControlGeneralConfig1.Load += new System.EventHandler(this.userControlGeneralConfig1_Load);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlExgConfig1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1033, 546);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ExG Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlExgConfig1
            // 
            this.userControlExgConfig1.Location = new System.Drawing.Point(8, 7);
            this.userControlExgConfig1.Margin = new System.Windows.Forms.Padding(5);
            this.userControlExgConfig1.Name = "userControlExgConfig1";
            this.userControlExgConfig1.Size = new System.Drawing.Size(1015, 571);
            this.userControlExgConfig1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.userControlSdConfig1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1033, 546);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SD Log";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // userControlSdConfig1
            // 
            this.userControlSdConfig1.Location = new System.Drawing.Point(0, 0);
            this.userControlSdConfig1.Margin = new System.Windows.Forms.Padding(5);
            this.userControlSdConfig1.MaximumSize = new System.Drawing.Size(1060, 542);
            this.userControlSdConfig1.MinimumSize = new System.Drawing.Size(1060, 542);
            this.userControlSdConfig1.Name = "userControlSdConfig1";
            this.userControlSdConfig1.Size = new System.Drawing.Size(1060, 542);
            this.userControlSdConfig1.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(876, 598);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(141, 31);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Apply All";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(727, 599);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(141, 31);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Exit";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 641);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.tabControl1);
            this.Icon = global::ShimmerAPI.Properties.Resources.ic_shimmercapture;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Configuration";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.Configuration_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        protected internal UserControlExgConfig userControlExgConfig1;
        protected internal UserControlGeneralConfig userControlGeneralConfig1;
        private System.Windows.Forms.TabPage tabPage3;
        private UserControlSdConfig userControlSdConfig1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}