namespace ShimmerAPI
{
    partial class UserControlSdConfig
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radBtnUserButton = new System.Windows.Forms.RadioButton();
            this.radBtnAutoRun = new System.Windows.Forms.RadioButton();
            this.radBtnSingletouch = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBxNshimmer = new System.Windows.Forms.TextBox();
            this.chBxSync = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBxMyID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtBxExpID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBxShimmerName = new System.Windows.Forms.TextBox();
            this.chBxIAmMaster = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBxCenter = new System.Windows.Forms.TextBox();
            this.txtBxConfigTime = new System.Windows.Forms.TextBox();
            this.buttonApplySd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownInterval)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // numUpDownInterval
            // 
            this.numUpDownInterval.Location = new System.Drawing.Point(99, 279);
            this.numUpDownInterval.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownInterval.Minimum = new decimal(new int[] {
            54,
            0,
            0,
            0});
            this.numUpDownInterval.Name = "numUpDownInterval";
            this.numUpDownInterval.Size = new System.Drawing.Size(43, 20);
            this.numUpDownInterval.TabIndex = 35;
            this.numUpDownInterval.Value = new decimal(new int[] {
            54,
            0,
            0,
            0});
            this.numUpDownInterval.ValueChanged += new System.EventHandler(this.numUpDownInterval_ValueChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radBtnUserButton);
            this.groupBox5.Controls.Add(this.radBtnAutoRun);
            this.groupBox5.Controls.Add(this.radBtnSingletouch);
            this.groupBox5.Location = new System.Drawing.Point(12, 162);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(192, 90);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Trigger Mode";
            // 
            // radBtnUserButton
            // 
            this.radBtnUserButton.AutoSize = true;
            this.radBtnUserButton.Location = new System.Drawing.Point(6, 42);
            this.radBtnUserButton.Name = "radBtnUserButton";
            this.radBtnUserButton.Size = new System.Drawing.Size(119, 17);
            this.radBtnUserButton.TabIndex = 20;
            this.radBtnUserButton.TabStop = true;
            this.radBtnUserButton.Text = "Press button to start";
            this.radBtnUserButton.UseVisualStyleBackColor = true;
            this.radBtnUserButton.CheckedChanged += new System.EventHandler(this.radBtnUserButton_CheckedChanged);
            // 
            // radBtnAutoRun
            // 
            this.radBtnAutoRun.AutoSize = true;
            this.radBtnAutoRun.Location = new System.Drawing.Point(6, 62);
            this.radBtnAutoRun.Name = "radBtnAutoRun";
            this.radBtnAutoRun.Size = new System.Drawing.Size(98, 17);
            this.radBtnAutoRun.TabIndex = 19;
            this.radBtnAutoRun.TabStop = true;
            this.radBtnAutoRun.Text = "Undock to start";
            this.radBtnAutoRun.UseVisualStyleBackColor = true;
            this.radBtnAutoRun.CheckedChanged += new System.EventHandler(this.radBtnAutoRun_CheckedChanged);
            // 
            // radBtnSingletouch
            // 
            this.radBtnSingletouch.AutoSize = true;
            this.radBtnSingletouch.Location = new System.Drawing.Point(6, 19);
            this.radBtnSingletouch.Name = "radBtnSingletouch";
            this.radBtnSingletouch.Size = new System.Drawing.Size(84, 17);
            this.radBtnSingletouch.TabIndex = 18;
            this.radBtnSingletouch.TabStop = true;
            this.radBtnSingletouch.Text = "Single touch";
            this.radBtnSingletouch.UseVisualStyleBackColor = true;
            this.radBtnSingletouch.CheckedChanged += new System.EventHandler(this.radBtnSingletouch_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "NShimmer";
            // 
            // txtBxNshimmer
            // 
            this.txtBxNshimmer.Location = new System.Drawing.Point(96, 138);
            this.txtBxNshimmer.Name = "txtBxNshimmer";
            this.txtBxNshimmer.Size = new System.Drawing.Size(108, 20);
            this.txtBxNshimmer.TabIndex = 30;
            this.txtBxNshimmer.TextChanged += new System.EventHandler(this.txtBxNshimmer_TextChanged);
            // 
            // chBxSync
            // 
            this.chBxSync.AutoSize = true;
            this.chBxSync.Location = new System.Drawing.Point(12, 258);
            this.chBxSync.Name = "chBxSync";
            this.chBxSync.Size = new System.Drawing.Size(118, 17);
            this.chBxSync.TabIndex = 33;
            this.chBxSync.Text = "Multi Shimmer Sync";
            this.chBxSync.UseVisualStyleBackColor = true;
            this.chBxSync.CheckedChanged += new System.EventHandler(this.chBxSync_CheckedChanged_1);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(40, 281);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Interval(s)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 114);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "My ID";
            // 
            // txtBxMyID
            // 
            this.txtBxMyID.Location = new System.Drawing.Point(96, 112);
            this.txtBxMyID.Name = "txtBxMyID";
            this.txtBxMyID.Size = new System.Drawing.Size(108, 20);
            this.txtBxMyID.TabIndex = 28;
            this.txtBxMyID.TextChanged += new System.EventHandler(this.txtBxMyID_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Experiment ID";
            // 
            // txtBxExpID
            // 
            this.txtBxExpID.Location = new System.Drawing.Point(96, 86);
            this.txtBxExpID.MaxLength = 12;
            this.txtBxExpID.Name = "txtBxExpID";
            this.txtBxExpID.Size = new System.Drawing.Size(108, 20);
            this.txtBxExpID.TabIndex = 26;
            this.txtBxExpID.TextChanged += new System.EventHandler(this.txtBxExpID_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Shimmer Name";
            // 
            // txtBxShimmerName
            // 
            this.txtBxShimmerName.Location = new System.Drawing.Point(96, 60);
            this.txtBxShimmerName.MaxLength = 12;
            this.txtBxShimmerName.Name = "txtBxShimmerName";
            this.txtBxShimmerName.Size = new System.Drawing.Size(108, 20);
            this.txtBxShimmerName.TabIndex = 24;
            this.txtBxShimmerName.TextChanged += new System.EventHandler(this.txtBxShimmerName_TextChanged);
            // 
            // chBxIAmMaster
            // 
            this.chBxIAmMaster.AutoSize = true;
            this.chBxIAmMaster.Location = new System.Drawing.Point(15, 12);
            this.chBxIAmMaster.Name = "chBxIAmMaster";
            this.chBxIAmMaster.Size = new System.Drawing.Size(132, 17);
            this.chBxIAmMaster.TabIndex = 23;
            this.chBxIAmMaster.Text = "This shimmer is Master";
            this.chBxIAmMaster.UseVisualStyleBackColor = true;
            this.chBxIAmMaster.CheckedChanged += new System.EventHandler(this.chBxIAmMaster_CheckedChanged_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Master Address";
            // 
            // txtBxCenter
            // 
            this.txtBxCenter.Location = new System.Drawing.Point(96, 33);
            this.txtBxCenter.MaxLength = 12;
            this.txtBxCenter.Name = "txtBxCenter";
            this.txtBxCenter.Size = new System.Drawing.Size(108, 20);
            this.txtBxCenter.TabIndex = 21;
            this.txtBxCenter.TextChanged += new System.EventHandler(this.txtBxCenter_TextChanged);
            // 
            // txtBxConfigTime
            // 
            this.txtBxConfigTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBxConfigTime.Location = new System.Drawing.Point(0, 334);
            this.txtBxConfigTime.Name = "txtBxConfigTime";
            this.txtBxConfigTime.Size = new System.Drawing.Size(204, 13);
            this.txtBxConfigTime.TabIndex = 37;
            // 
            // buttonApplySd
            // 
            this.buttonApplySd.Location = new System.Drawing.Point(641, 407);
            this.buttonApplySd.Name = "buttonApplySd";
            this.buttonApplySd.Size = new System.Drawing.Size(106, 25);
            this.buttonApplySd.TabIndex = 38;
            this.buttonApplySd.Text = "Apply";
            this.buttonApplySd.UseVisualStyleBackColor = true;
            this.buttonApplySd.Click += new System.EventHandler(this.buttonApplySd_Click);
            // 
            // UserControlSdConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonApplySd);
            this.Controls.Add(this.txtBxConfigTime);
            this.Controls.Add(this.numUpDownInterval);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtBxNshimmer);
            this.Controls.Add(this.chBxSync);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBxMyID);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtBxExpID);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtBxShimmerName);
            this.Controls.Add(this.chBxIAmMaster);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtBxCenter);
            this.MaximumSize = new System.Drawing.Size(795, 440);
            this.MinimumSize = new System.Drawing.Size(795, 440);
            this.Name = "UserControlSdConfig";
            this.Size = new System.Drawing.Size(795, 440);
            this.Load += new System.EventHandler(this.UserControlSdConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownInterval)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numUpDownInterval;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radBtnUserButton;
        private System.Windows.Forms.RadioButton radBtnAutoRun;
        private System.Windows.Forms.RadioButton radBtnSingletouch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBxNshimmer;
        private System.Windows.Forms.CheckBox chBxSync;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBxMyID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtBxExpID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBxShimmerName;
        private System.Windows.Forms.CheckBox chBxIAmMaster;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBxCenter;
        private System.Windows.Forms.TextBox txtBxConfigTime;
        private System.Windows.Forms.Button buttonApplySd;
    }
}
