namespace ShimmerAPI
{
    partial class ExgConfigure
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
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.checkBoxBSF60 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF0_05 = new System.Windows.Forms.CheckBox();
            this.checkBoxBSF50 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF0_5 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF5 = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.textBoxChip2Reg10 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg1 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg9 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg10 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg8 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg5 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg7 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg9 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg6 = new System.Windows.Forms.TextBox();
            this.labelChip2 = new System.Windows.Forms.Label();
            this.textBoxChip2Reg5 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg8 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg4 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg4 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg3 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg2 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg7 = new System.Windows.Forms.TextBox();
            this.comboBoxChip2Channel2Gain = new System.Windows.Forms.ComboBox();
            this.textBoxChip1Reg6 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg3 = new System.Windows.Forms.TextBox();
            this.labelChip1 = new System.Windows.Forms.Label();
            this.textBoxChip1Reg2 = new System.Windows.Forms.TextBox();
            this.labelChip2Channel2 = new System.Windows.Forms.Label();
            this.textBoxChip1Reg1 = new System.Windows.Forms.TextBox();
            this.comboBoxChip2Channel1Gain = new System.Windows.Forms.ComboBox();
            this.checkBoxTestSignal = new System.Windows.Forms.CheckBox();
            this.comboBoxChip1Channel2Gain = new System.Windows.Forms.ComboBox();
            this.labelChip2Channel1 = new System.Windows.Forms.Label();
            this.comboBoxChip1Channel1Gain = new System.Windows.Forms.ComboBox();
            this.checkBoxDefaultEMG = new System.Windows.Forms.CheckBox();
            this.labelChip1Channel2 = new System.Windows.Forms.Label();
            this.checkBoxDefaultECG = new System.Windows.Forms.CheckBox();
            this.labelChip1Channel1 = new System.Windows.Forms.Label();
            this.groupBoxFilters.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.Controls.Add(this.checkBoxBSF60);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF0_05);
            this.groupBoxFilters.Controls.Add(this.checkBoxBSF50);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF0_5);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF5);
            this.groupBoxFilters.Location = new System.Drawing.Point(12, 22);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Size = new System.Drawing.Size(421, 103);
            this.groupBoxFilters.TabIndex = 0;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "Filtering Options";
            // 
            // checkBoxBSF60
            // 
            this.checkBoxBSF60.AutoSize = true;
            this.checkBoxBSF60.Location = new System.Drawing.Point(156, 60);
            this.checkBoxBSF60.Name = "checkBoxBSF60";
            this.checkBoxBSF60.Size = new System.Drawing.Size(125, 17);
            this.checkBoxBSF60.TabIndex = 5;
            this.checkBoxBSF60.Text = "Enable BSF 59-61Hz";
            this.checkBoxBSF60.UseVisualStyleBackColor = true;
            this.checkBoxBSF60.Click += new System.EventHandler(this.checkBoxBSF60_Click);
            // 
            // checkBoxHPF0_05
            // 
            this.checkBoxHPF0_05.AutoSize = true;
            this.checkBoxHPF0_05.Location = new System.Drawing.Point(6, 25);
            this.checkBoxHPF0_05.Name = "checkBoxHPF0_05";
            this.checkBoxHPF0_05.Size = new System.Drawing.Size(120, 17);
            this.checkBoxHPF0_05.TabIndex = 1;
            this.checkBoxHPF0_05.Text = "Enable HPF 0.05Hz";
            this.checkBoxHPF0_05.UseVisualStyleBackColor = true;
            this.checkBoxHPF0_05.Click += new System.EventHandler(this.checkBoxHPF0_05_Click);
            // 
            // checkBoxBSF50
            // 
            this.checkBoxBSF50.AutoSize = true;
            this.checkBoxBSF50.Location = new System.Drawing.Point(6, 60);
            this.checkBoxBSF50.Name = "checkBoxBSF50";
            this.checkBoxBSF50.Size = new System.Drawing.Size(125, 17);
            this.checkBoxBSF50.TabIndex = 4;
            this.checkBoxBSF50.Text = "Enable BSF 49-51Hz";
            this.checkBoxBSF50.UseVisualStyleBackColor = true;
            this.checkBoxBSF50.Click += new System.EventHandler(this.checkBoxBSF50_Click);
            // 
            // checkBoxHPF0_5
            // 
            this.checkBoxHPF0_5.AutoSize = true;
            this.checkBoxHPF0_5.Location = new System.Drawing.Point(156, 25);
            this.checkBoxHPF0_5.Name = "checkBoxHPF0_5";
            this.checkBoxHPF0_5.Size = new System.Drawing.Size(114, 17);
            this.checkBoxHPF0_5.TabIndex = 2;
            this.checkBoxHPF0_5.Text = "Enable HPF 0.5Hz";
            this.checkBoxHPF0_5.UseVisualStyleBackColor = true;
            this.checkBoxHPF0_5.Click += new System.EventHandler(this.checkBoxHPF0_5_Click);
            // 
            // checkBoxHPF5
            // 
            this.checkBoxHPF5.AutoSize = true;
            this.checkBoxHPF5.Location = new System.Drawing.Point(306, 25);
            this.checkBoxHPF5.Name = "checkBoxHPF5";
            this.checkBoxHPF5.Size = new System.Drawing.Size(105, 17);
            this.checkBoxHPF5.TabIndex = 3;
            this.checkBoxHPF5.Text = "Enable HPF 5Hz";
            this.checkBoxHPF5.UseVisualStyleBackColor = true;
            this.checkBoxHPF5.Click += new System.EventHandler(this.checkBoxHPF5_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(83, 374);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(95, 31);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(244, 374);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 31);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg10);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg1);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg9);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg10);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg8);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg5);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg7);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg9);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg6);
            this.groupBoxSettings.Controls.Add(this.labelChip2);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg5);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg8);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg4);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg4);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg3);
            this.groupBoxSettings.Controls.Add(this.textBoxChip2Reg2);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg7);
            this.groupBoxSettings.Controls.Add(this.comboBoxChip2Channel2Gain);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg6);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg3);
            this.groupBoxSettings.Controls.Add(this.labelChip1);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg2);
            this.groupBoxSettings.Controls.Add(this.labelChip2Channel2);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg1);
            this.groupBoxSettings.Controls.Add(this.comboBoxChip2Channel1Gain);
            this.groupBoxSettings.Controls.Add(this.checkBoxTestSignal);
            this.groupBoxSettings.Controls.Add(this.comboBoxChip1Channel2Gain);
            this.groupBoxSettings.Controls.Add(this.labelChip2Channel1);
            this.groupBoxSettings.Controls.Add(this.comboBoxChip1Channel1Gain);
            this.groupBoxSettings.Controls.Add(this.checkBoxDefaultEMG);
            this.groupBoxSettings.Controls.Add(this.labelChip1Channel2);
            this.groupBoxSettings.Controls.Add(this.checkBoxDefaultECG);
            this.groupBoxSettings.Controls.Add(this.labelChip1Channel1);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 147);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(421, 187);
            this.groupBoxSettings.TabIndex = 3;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "ExG Settings";
            // 
            // textBoxChip2Reg10
            // 
            this.textBoxChip2Reg10.Location = new System.Drawing.Point(322, 153);
            this.textBoxChip2Reg10.Name = "textBoxChip2Reg10";
            this.textBoxChip2Reg10.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg10.TabIndex = 17;
            // 
            // textBoxChip2Reg1
            // 
            this.textBoxChip2Reg1.Location = new System.Drawing.Point(52, 153);
            this.textBoxChip2Reg1.Name = "textBoxChip2Reg1";
            this.textBoxChip2Reg1.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg1.TabIndex = 9;
            // 
            // textBoxChip2Reg9
            // 
            this.textBoxChip2Reg9.Location = new System.Drawing.Point(292, 153);
            this.textBoxChip2Reg9.Name = "textBoxChip2Reg9";
            this.textBoxChip2Reg9.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg9.TabIndex = 16;
            // 
            // textBoxChip1Reg10
            // 
            this.textBoxChip1Reg10.Location = new System.Drawing.Point(322, 118);
            this.textBoxChip1Reg10.Name = "textBoxChip1Reg10";
            this.textBoxChip1Reg10.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg10.TabIndex = 8;
            // 
            // textBoxChip2Reg8
            // 
            this.textBoxChip2Reg8.Location = new System.Drawing.Point(262, 153);
            this.textBoxChip2Reg8.Name = "textBoxChip2Reg8";
            this.textBoxChip2Reg8.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg8.TabIndex = 15;
            // 
            // textBoxChip1Reg5
            // 
            this.textBoxChip1Reg5.Location = new System.Drawing.Point(172, 118);
            this.textBoxChip1Reg5.Name = "textBoxChip1Reg5";
            this.textBoxChip1Reg5.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg5.TabIndex = 8;
            // 
            // textBoxChip2Reg7
            // 
            this.textBoxChip2Reg7.Location = new System.Drawing.Point(232, 153);
            this.textBoxChip2Reg7.Name = "textBoxChip2Reg7";
            this.textBoxChip2Reg7.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg7.TabIndex = 14;
            // 
            // textBoxChip1Reg9
            // 
            this.textBoxChip1Reg9.Location = new System.Drawing.Point(292, 118);
            this.textBoxChip1Reg9.Name = "textBoxChip1Reg9";
            this.textBoxChip1Reg9.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg9.TabIndex = 7;
            // 
            // textBoxChip2Reg6
            // 
            this.textBoxChip2Reg6.Location = new System.Drawing.Point(202, 153);
            this.textBoxChip2Reg6.Name = "textBoxChip2Reg6";
            this.textBoxChip2Reg6.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg6.TabIndex = 13;
            // 
            // labelChip2
            // 
            this.labelChip2.AutoSize = true;
            this.labelChip2.Location = new System.Drawing.Point(6, 156);
            this.labelChip2.Name = "labelChip2";
            this.labelChip2.Size = new System.Drawing.Size(40, 13);
            this.labelChip2.TabIndex = 5;
            this.labelChip2.Text = "Chip 2:";
            // 
            // textBoxChip2Reg5
            // 
            this.textBoxChip2Reg5.Location = new System.Drawing.Point(172, 153);
            this.textBoxChip2Reg5.Name = "textBoxChip2Reg5";
            this.textBoxChip2Reg5.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg5.TabIndex = 12;
            // 
            // textBoxChip1Reg8
            // 
            this.textBoxChip1Reg8.Location = new System.Drawing.Point(262, 118);
            this.textBoxChip1Reg8.Name = "textBoxChip1Reg8";
            this.textBoxChip1Reg8.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg8.TabIndex = 6;
            // 
            // textBoxChip2Reg4
            // 
            this.textBoxChip2Reg4.Location = new System.Drawing.Point(142, 153);
            this.textBoxChip2Reg4.Name = "textBoxChip2Reg4";
            this.textBoxChip2Reg4.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg4.TabIndex = 11;
            // 
            // textBoxChip1Reg4
            // 
            this.textBoxChip1Reg4.Location = new System.Drawing.Point(142, 118);
            this.textBoxChip1Reg4.Name = "textBoxChip1Reg4";
            this.textBoxChip1Reg4.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg4.TabIndex = 7;
            // 
            // textBoxChip2Reg3
            // 
            this.textBoxChip2Reg3.Location = new System.Drawing.Point(112, 153);
            this.textBoxChip2Reg3.Name = "textBoxChip2Reg3";
            this.textBoxChip2Reg3.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg3.TabIndex = 4;
            // 
            // textBoxChip2Reg2
            // 
            this.textBoxChip2Reg2.Location = new System.Drawing.Point(82, 153);
            this.textBoxChip2Reg2.Name = "textBoxChip2Reg2";
            this.textBoxChip2Reg2.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg2.TabIndex = 10;
            // 
            // textBoxChip1Reg7
            // 
            this.textBoxChip1Reg7.Location = new System.Drawing.Point(232, 118);
            this.textBoxChip1Reg7.Name = "textBoxChip1Reg7";
            this.textBoxChip1Reg7.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg7.TabIndex = 5;
            // 
            // comboBoxChip2Channel2Gain
            // 
            this.comboBoxChip2Channel2Gain.FormattingEnabled = true;
            this.comboBoxChip2Channel2Gain.Location = new System.Drawing.Point(315, 76);
            this.comboBoxChip2Channel2Gain.Name = "comboBoxChip2Channel2Gain";
            this.comboBoxChip2Channel2Gain.Size = new System.Drawing.Size(94, 21);
            this.comboBoxChip2Channel2Gain.TabIndex = 7;
            this.comboBoxChip2Channel2Gain.SelectedIndexChanged += new System.EventHandler(this.comboBoxChip2Channel2Gain_SelectedIndexChanged);
            // 
            // textBoxChip1Reg6
            // 
            this.textBoxChip1Reg6.Location = new System.Drawing.Point(202, 118);
            this.textBoxChip1Reg6.Name = "textBoxChip1Reg6";
            this.textBoxChip1Reg6.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg6.TabIndex = 4;
            // 
            // textBoxChip1Reg3
            // 
            this.textBoxChip1Reg3.Location = new System.Drawing.Point(112, 118);
            this.textBoxChip1Reg3.Name = "textBoxChip1Reg3";
            this.textBoxChip1Reg3.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg3.TabIndex = 6;
            // 
            // labelChip1
            // 
            this.labelChip1.AutoSize = true;
            this.labelChip1.Location = new System.Drawing.Point(6, 121);
            this.labelChip1.Name = "labelChip1";
            this.labelChip1.Size = new System.Drawing.Size(40, 13);
            this.labelChip1.TabIndex = 4;
            this.labelChip1.Text = "Chip 1:";
            // 
            // textBoxChip1Reg2
            // 
            this.textBoxChip1Reg2.Location = new System.Drawing.Point(82, 118);
            this.textBoxChip1Reg2.Name = "textBoxChip1Reg2";
            this.textBoxChip1Reg2.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg2.TabIndex = 5;
            // 
            // labelChip2Channel2
            // 
            this.labelChip2Channel2.AutoSize = true;
            this.labelChip2Channel2.Location = new System.Drawing.Point(312, 60);
            this.labelChip2Channel2.Name = "labelChip2Channel2";
            this.labelChip2Channel2.Size = new System.Drawing.Size(84, 13);
            this.labelChip2Channel2.TabIndex = 7;
            this.labelChip2Channel2.Text = "EXG2 CH2 Gain";
            // 
            // textBoxChip1Reg1
            // 
            this.textBoxChip1Reg1.Location = new System.Drawing.Point(52, 118);
            this.textBoxChip1Reg1.Name = "textBoxChip1Reg1";
            this.textBoxChip1Reg1.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg1.TabIndex = 4;
            // 
            // comboBoxChip2Channel1Gain
            // 
            this.comboBoxChip2Channel1Gain.FormattingEnabled = true;
            this.comboBoxChip2Channel1Gain.Location = new System.Drawing.Point(212, 76);
            this.comboBoxChip2Channel1Gain.Name = "comboBoxChip2Channel1Gain";
            this.comboBoxChip2Channel1Gain.Size = new System.Drawing.Size(94, 21);
            this.comboBoxChip2Channel1Gain.TabIndex = 6;
            this.comboBoxChip2Channel1Gain.SelectedIndexChanged += new System.EventHandler(this.comboBoxChip2Channel1Gain_SelectedIndexChanged);
            // 
            // checkBoxTestSignal
            // 
            this.checkBoxTestSignal.AutoSize = true;
            this.checkBoxTestSignal.Location = new System.Drawing.Point(306, 25);
            this.checkBoxTestSignal.Name = "checkBoxTestSignal";
            this.checkBoxTestSignal.Size = new System.Drawing.Size(79, 17);
            this.checkBoxTestSignal.TabIndex = 2;
            this.checkBoxTestSignal.Text = "Test Signal";
            this.checkBoxTestSignal.UseVisualStyleBackColor = true;
            this.checkBoxTestSignal.Click += new System.EventHandler(this.checkBoxTestSignal_Click);
            // 
            // comboBoxChip1Channel2Gain
            // 
            this.comboBoxChip1Channel2Gain.FormattingEnabled = true;
            this.comboBoxChip1Channel2Gain.Location = new System.Drawing.Point(109, 76);
            this.comboBoxChip1Channel2Gain.Name = "comboBoxChip1Channel2Gain";
            this.comboBoxChip1Channel2Gain.Size = new System.Drawing.Size(94, 21);
            this.comboBoxChip1Channel2Gain.TabIndex = 5;
            this.comboBoxChip1Channel2Gain.SelectedIndexChanged += new System.EventHandler(this.comboBoxChip1Channel2Gain_SelectedIndexChanged);
            // 
            // labelChip2Channel1
            // 
            this.labelChip2Channel1.AutoSize = true;
            this.labelChip2Channel1.Location = new System.Drawing.Point(209, 60);
            this.labelChip2Channel1.Name = "labelChip2Channel1";
            this.labelChip2Channel1.Size = new System.Drawing.Size(84, 13);
            this.labelChip2Channel1.TabIndex = 6;
            this.labelChip2Channel1.Text = "EXG2 CH1 Gain";
            // 
            // comboBoxChip1Channel1Gain
            // 
            this.comboBoxChip1Channel1Gain.FormattingEnabled = true;
            this.comboBoxChip1Channel1Gain.Location = new System.Drawing.Point(6, 76);
            this.comboBoxChip1Channel1Gain.Name = "comboBoxChip1Channel1Gain";
            this.comboBoxChip1Channel1Gain.Size = new System.Drawing.Size(94, 21);
            this.comboBoxChip1Channel1Gain.TabIndex = 4;
            this.comboBoxChip1Channel1Gain.SelectedIndexChanged += new System.EventHandler(this.comboBoxChip1Channel1Gain_SelectedIndexChanged);
            // 
            // checkBoxDefaultEMG
            // 
            this.checkBoxDefaultEMG.AutoSize = true;
            this.checkBoxDefaultEMG.Location = new System.Drawing.Point(156, 25);
            this.checkBoxDefaultEMG.Name = "checkBoxDefaultEMG";
            this.checkBoxDefaultEMG.Size = new System.Drawing.Size(120, 17);
            this.checkBoxDefaultEMG.TabIndex = 1;
            this.checkBoxDefaultEMG.Text = "Default EMG Config";
            this.checkBoxDefaultEMG.UseVisualStyleBackColor = true;
            this.checkBoxDefaultEMG.Click += new System.EventHandler(this.checkBoxDefaultEMG_Click);
            // 
            // labelChip1Channel2
            // 
            this.labelChip1Channel2.AutoSize = true;
            this.labelChip1Channel2.Location = new System.Drawing.Point(106, 60);
            this.labelChip1Channel2.Name = "labelChip1Channel2";
            this.labelChip1Channel2.Size = new System.Drawing.Size(84, 13);
            this.labelChip1Channel2.TabIndex = 5;
            this.labelChip1Channel2.Text = "EXG1 CH2 Gain";
            // 
            // checkBoxDefaultECG
            // 
            this.checkBoxDefaultECG.AutoSize = true;
            this.checkBoxDefaultECG.Location = new System.Drawing.Point(6, 25);
            this.checkBoxDefaultECG.Name = "checkBoxDefaultECG";
            this.checkBoxDefaultECG.Size = new System.Drawing.Size(118, 17);
            this.checkBoxDefaultECG.TabIndex = 0;
            this.checkBoxDefaultECG.Text = "Default ECG Config";
            this.checkBoxDefaultECG.UseVisualStyleBackColor = true;
            this.checkBoxDefaultECG.Click += new System.EventHandler(this.checkBoxDefaultECG_Click);
            // 
            // labelChip1Channel1
            // 
            this.labelChip1Channel1.AutoSize = true;
            this.labelChip1Channel1.Location = new System.Drawing.Point(6, 60);
            this.labelChip1Channel1.Name = "labelChip1Channel1";
            this.labelChip1Channel1.Size = new System.Drawing.Size(84, 13);
            this.labelChip1Channel1.TabIndex = 4;
            this.labelChip1Channel1.Text = "EXG1 CH1 Gain";
            // 
            // ExgConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxFilters);
            this.Name = "ExgConfigure";
            this.Size = new System.Drawing.Size(445, 436);
            this.Load += new System.EventHandler(this.ExgConfigure_Load);
            this.groupBoxFilters.ResumeLayout(false);
            this.groupBoxFilters.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.CheckBox checkBoxBSF60;
        private System.Windows.Forms.CheckBox checkBoxHPF0_05;
        private System.Windows.Forms.CheckBox checkBoxBSF50;
        private System.Windows.Forms.CheckBox checkBoxHPF0_5;
        private System.Windows.Forms.CheckBox checkBoxHPF5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.CheckBox checkBoxTestSignal;
        private System.Windows.Forms.CheckBox checkBoxDefaultEMG;
        private System.Windows.Forms.CheckBox checkBoxDefaultECG;
        private System.Windows.Forms.Label labelChip2Channel2;
        private System.Windows.Forms.Label labelChip2Channel1;
        private System.Windows.Forms.Label labelChip1Channel2;
        private System.Windows.Forms.Label labelChip1Channel1;
        private System.Windows.Forms.ComboBox comboBoxChip2Channel2Gain;
        private System.Windows.Forms.ComboBox comboBoxChip2Channel1Gain;
        private System.Windows.Forms.ComboBox comboBoxChip1Channel2Gain;
        private System.Windows.Forms.ComboBox comboBoxChip1Channel1Gain;
        private System.Windows.Forms.Label labelChip1;
        private System.Windows.Forms.Label labelChip2;
        private System.Windows.Forms.TextBox textBoxChip1Reg1;
        private System.Windows.Forms.TextBox textBoxChip1Reg2;
        private System.Windows.Forms.TextBox textBoxChip1Reg3;
        private System.Windows.Forms.TextBox textBoxChip1Reg4;
        private System.Windows.Forms.TextBox textBoxChip1Reg5;
        private System.Windows.Forms.TextBox textBoxChip1Reg6;
        private System.Windows.Forms.TextBox textBoxChip1Reg7;
        private System.Windows.Forms.TextBox textBoxChip1Reg8;
        private System.Windows.Forms.TextBox textBoxChip1Reg9;
        private System.Windows.Forms.TextBox textBoxChip1Reg10;
        private System.Windows.Forms.TextBox textBoxChip2Reg1;
        private System.Windows.Forms.TextBox textBoxChip2Reg2;
        private System.Windows.Forms.TextBox textBoxChip2Reg3;
        private System.Windows.Forms.TextBox textBoxChip2Reg4;
        private System.Windows.Forms.TextBox textBoxChip2Reg5;
        private System.Windows.Forms.TextBox textBoxChip2Reg6;
        private System.Windows.Forms.TextBox textBoxChip2Reg7;
        private System.Windows.Forms.TextBox textBoxChip2Reg8;
        private System.Windows.Forms.TextBox textBoxChip2Reg9;
        private System.Windows.Forms.TextBox textBoxChip2Reg10;
    }
}