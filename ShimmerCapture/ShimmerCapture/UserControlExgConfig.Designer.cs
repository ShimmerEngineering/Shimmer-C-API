namespace ShimmerAPI
{
    partial class UserControlExgConfig
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
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.checkBoxNQFilter = new System.Windows.Forms.CheckBox();
            this.checkBoxBSF60 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF0_05 = new System.Windows.Forms.CheckBox();
            this.checkBoxBSF50 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF0_5 = new System.Windows.Forms.CheckBox();
            this.checkBoxHPF5 = new System.Windows.Forms.CheckBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxChip1Reg1 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg2 = new System.Windows.Forms.TextBox();
            this.labelChip1 = new System.Windows.Forms.Label();
            this.textBoxChip1Reg3 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg6 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg7 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg2 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg3 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg4 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg4 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg8 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg5 = new System.Windows.Forms.TextBox();
            this.labelChip2 = new System.Windows.Forms.Label();
            this.textBoxChip2Reg6 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg9 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg7 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg5 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg8 = new System.Windows.Forms.TextBox();
            this.textBoxChip1Reg10 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg9 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg1 = new System.Windows.Forms.TextBox();
            this.textBoxChip2Reg10 = new System.Windows.Forms.TextBox();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxDefaultExGTest = new System.Windows.Forms.CheckBox();
            this.checkBoxDefaultEMG = new System.Windows.Forms.CheckBox();
            this.checkBoxDefaultECG = new System.Windows.Forms.CheckBox();
            this.labelEXG = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxExGReferenceElectrode = new System.Windows.Forms.ComboBox();
            this.labelExGReferenceElectrode = new System.Windows.Forms.Label();
            this.comboBoxLeadOffDetection = new System.Windows.Forms.ComboBox();
            this.labelExGLeadOffDetection = new System.Windows.Forms.Label();
            this.comboBoxExGLeadOffCurrent = new System.Windows.Forms.ComboBox();
            this.comboBoxLeadOffComparatorThreshold = new System.Windows.Forms.ComboBox();
            this.labelExGLeadOffCurrent = new System.Windows.Forms.Label();
            this.labelExGLeadOffComparatorThreshold = new System.Windows.Forms.Label();
            this.groupBoxFilters.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.Controls.Add(this.checkBoxNQFilter);
            this.groupBoxFilters.Controls.Add(this.checkBoxBSF60);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF0_05);
            this.groupBoxFilters.Controls.Add(this.checkBoxBSF50);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF0_5);
            this.groupBoxFilters.Controls.Add(this.checkBoxHPF5);
            this.groupBoxFilters.Location = new System.Drawing.Point(12, 22);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Size = new System.Drawing.Size(481, 93);
            this.groupBoxFilters.TabIndex = 0;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "Filtering Options";
            // 
            // checkBoxNQFilter
            // 
            this.checkBoxNQFilter.AutoSize = true;
            this.checkBoxNQFilter.Location = new System.Drawing.Point(6, 71);
            this.checkBoxNQFilter.Name = "checkBoxNQFilter";
            this.checkBoxNQFilter.Size = new System.Drawing.Size(176, 17);
            this.checkBoxNQFilter.TabIndex = 8;
            this.checkBoxNQFilter.Text = "Enable Nyquist Rate Filter (LPF)";
            this.checkBoxNQFilter.UseVisualStyleBackColor = true;
            this.checkBoxNQFilter.CheckedChanged += new System.EventHandler(this.checkBoxNQFilter_CheckedChanged);
            // 
            // checkBoxBSF60
            // 
            this.checkBoxBSF60.AutoSize = true;
            this.checkBoxBSF60.Location = new System.Drawing.Point(181, 48);
            this.checkBoxBSF60.Name = "checkBoxBSF60";
            this.checkBoxBSF60.Size = new System.Drawing.Size(125, 17);
            this.checkBoxBSF60.TabIndex = 5;
            this.checkBoxBSF60.Text = "Enable BSF 59-61Hz";
            this.checkBoxBSF60.UseVisualStyleBackColor = true;
            this.checkBoxBSF60.CheckedChanged += new System.EventHandler(this.checkBoxBSF60_CheckedChanged);
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
            this.checkBoxHPF0_05.CheckedChanged += new System.EventHandler(this.checkBoxHPF0_05_CheckedChanged);
            this.checkBoxHPF0_05.Click += new System.EventHandler(this.checkBoxHPF0_05_Click);
            // 
            // checkBoxBSF50
            // 
            this.checkBoxBSF50.AutoSize = true;
            this.checkBoxBSF50.Location = new System.Drawing.Point(6, 48);
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
            this.checkBoxHPF0_5.Location = new System.Drawing.Point(181, 25);
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
            this.checkBoxHPF5.Location = new System.Drawing.Point(356, 25);
            this.checkBoxHPF5.Name = "checkBoxHPF5";
            this.checkBoxHPF5.Size = new System.Drawing.Size(105, 17);
            this.checkBoxHPF5.TabIndex = 3;
            this.checkBoxHPF5.Text = "Enable HPF 5Hz";
            this.checkBoxHPF5.UseVisualStyleBackColor = true;
            this.checkBoxHPF5.Click += new System.EventHandler(this.checkBoxHPF5_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(635, 401);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(106, 25);
            this.buttonApply.TabIndex = 7;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBoxChip1Reg1
            // 
            this.textBoxChip1Reg1.Location = new System.Drawing.Point(59, 111);
            this.textBoxChip1Reg1.Name = "textBoxChip1Reg1";
            this.textBoxChip1Reg1.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg1.TabIndex = 19;
            this.textBoxChip1Reg1.TextChanged += new System.EventHandler(this.textBoxChip1Reg1_TextChanged);
            this.textBoxChip1Reg1.Leave += new System.EventHandler(this.TextBoxExg1Reg1_OnLeave);
            // 
            // textBoxChip1Reg2
            // 
            this.textBoxChip1Reg2.Location = new System.Drawing.Point(93, 111);
            this.textBoxChip1Reg2.Name = "textBoxChip1Reg2";
            this.textBoxChip1Reg2.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg2.TabIndex = 20;
            this.textBoxChip1Reg2.TextChanged += new System.EventHandler(this.textBoxChip1Reg2_TextChanged);
            this.textBoxChip1Reg2.Leave += new System.EventHandler(this.TextBoxExg1Reg2_OnLeave);
            // 
            // labelChip1
            // 
            this.labelChip1.AutoSize = true;
            this.labelChip1.Location = new System.Drawing.Point(13, 114);
            this.labelChip1.Name = "labelChip1";
            this.labelChip1.Size = new System.Drawing.Size(40, 13);
            this.labelChip1.TabIndex = 18;
            this.labelChip1.Text = "Chip 1:";
            // 
            // textBoxChip1Reg3
            // 
            this.textBoxChip1Reg3.Location = new System.Drawing.Point(127, 111);
            this.textBoxChip1Reg3.Name = "textBoxChip1Reg3";
            this.textBoxChip1Reg3.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg3.TabIndex = 21;
            this.textBoxChip1Reg3.TextChanged += new System.EventHandler(this.textBoxChip1Reg3_TextChanged);
            this.textBoxChip1Reg3.Leave += new System.EventHandler(this.TextBoxExg1Reg3_OnLeave);
            // 
            // textBoxChip1Reg6
            // 
            this.textBoxChip1Reg6.Location = new System.Drawing.Point(229, 111);
            this.textBoxChip1Reg6.Name = "textBoxChip1Reg6";
            this.textBoxChip1Reg6.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg6.TabIndex = 24;
            this.textBoxChip1Reg6.TextChanged += new System.EventHandler(this.textBoxChip1Reg6_TextChanged);
            this.textBoxChip1Reg6.Leave += new System.EventHandler(this.TextBoxExg1Reg6_OnLeave);
            // 
            // textBoxChip1Reg7
            // 
            this.textBoxChip1Reg7.Location = new System.Drawing.Point(263, 111);
            this.textBoxChip1Reg7.Name = "textBoxChip1Reg7";
            this.textBoxChip1Reg7.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg7.TabIndex = 25;
            this.textBoxChip1Reg7.TextChanged += new System.EventHandler(this.textBoxChip1Reg7_TextChanged);
            this.textBoxChip1Reg7.Leave += new System.EventHandler(this.TextBoxExg1Reg7_OnLeave);
            // 
            // textBoxChip2Reg2
            // 
            this.textBoxChip2Reg2.Location = new System.Drawing.Point(93, 146);
            this.textBoxChip2Reg2.Name = "textBoxChip2Reg2";
            this.textBoxChip2Reg2.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg2.TabIndex = 31;
            this.textBoxChip2Reg2.TextChanged += new System.EventHandler(this.textBoxChip2Reg2_TextChanged);
            this.textBoxChip2Reg2.Leave += new System.EventHandler(this.TextBoxExg2Reg2_OnLeave);
            // 
            // textBoxChip2Reg3
            // 
            this.textBoxChip2Reg3.Location = new System.Drawing.Point(127, 146);
            this.textBoxChip2Reg3.Name = "textBoxChip2Reg3";
            this.textBoxChip2Reg3.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg3.TabIndex = 32;
            this.textBoxChip2Reg3.TextChanged += new System.EventHandler(this.textBoxChip2Reg3_TextChanged);
            this.textBoxChip2Reg3.Leave += new System.EventHandler(this.TextBoxExg2Reg3_OnLeave);
            // 
            // textBoxChip1Reg4
            // 
            this.textBoxChip1Reg4.Location = new System.Drawing.Point(161, 111);
            this.textBoxChip1Reg4.Name = "textBoxChip1Reg4";
            this.textBoxChip1Reg4.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg4.TabIndex = 22;
            this.textBoxChip1Reg4.TextChanged += new System.EventHandler(this.textBoxChip1Reg4_TextChanged);
            this.textBoxChip1Reg4.Leave += new System.EventHandler(this.TextBoxExg1Reg4_OnLeave);
            // 
            // textBoxChip2Reg4
            // 
            this.textBoxChip2Reg4.Location = new System.Drawing.Point(161, 146);
            this.textBoxChip2Reg4.Name = "textBoxChip2Reg4";
            this.textBoxChip2Reg4.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg4.TabIndex = 33;
            this.textBoxChip2Reg4.TextChanged += new System.EventHandler(this.textBoxChip2Reg4_TextChanged);
            this.textBoxChip2Reg4.Leave += new System.EventHandler(this.TextBoxExg2Reg4);
            // 
            // textBoxChip1Reg8
            // 
            this.textBoxChip1Reg8.Location = new System.Drawing.Point(297, 111);
            this.textBoxChip1Reg8.Name = "textBoxChip1Reg8";
            this.textBoxChip1Reg8.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg8.TabIndex = 26;
            this.textBoxChip1Reg8.TextChanged += new System.EventHandler(this.textBoxChip1Reg8_TextChanged);
            this.textBoxChip1Reg8.Leave += new System.EventHandler(this.TextBoxExg1Reg8_OnLeave);
            // 
            // textBoxChip2Reg5
            // 
            this.textBoxChip2Reg5.Location = new System.Drawing.Point(195, 146);
            this.textBoxChip2Reg5.Name = "textBoxChip2Reg5";
            this.textBoxChip2Reg5.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg5.TabIndex = 34;
            this.textBoxChip2Reg5.TextChanged += new System.EventHandler(this.textBoxChip2Reg5_TextChanged);
            this.textBoxChip2Reg5.Leave += new System.EventHandler(this.TextBoxExg2Reg5);
            // 
            // labelChip2
            // 
            this.labelChip2.AutoSize = true;
            this.labelChip2.Location = new System.Drawing.Point(13, 149);
            this.labelChip2.Name = "labelChip2";
            this.labelChip2.Size = new System.Drawing.Size(40, 13);
            this.labelChip2.TabIndex = 29;
            this.labelChip2.Text = "Chip 2:";
            // 
            // textBoxChip2Reg6
            // 
            this.textBoxChip2Reg6.Location = new System.Drawing.Point(229, 146);
            this.textBoxChip2Reg6.Name = "textBoxChip2Reg6";
            this.textBoxChip2Reg6.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg6.TabIndex = 35;
            this.textBoxChip2Reg6.TextChanged += new System.EventHandler(this.textBoxChip2Reg6_TextChanged);
            this.textBoxChip2Reg6.Leave += new System.EventHandler(this.TextBoxExg2Reg6);
            // 
            // textBoxChip1Reg9
            // 
            this.textBoxChip1Reg9.Location = new System.Drawing.Point(331, 111);
            this.textBoxChip1Reg9.Name = "textBoxChip1Reg9";
            this.textBoxChip1Reg9.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg9.TabIndex = 27;
            this.textBoxChip1Reg9.TextChanged += new System.EventHandler(this.textBoxChip1Reg9_TextChanged);
            this.textBoxChip1Reg9.Leave += new System.EventHandler(this.TextBoxExg1Reg9_OnLeave);
            // 
            // textBoxChip2Reg7
            // 
            this.textBoxChip2Reg7.Location = new System.Drawing.Point(263, 146);
            this.textBoxChip2Reg7.Name = "textBoxChip2Reg7";
            this.textBoxChip2Reg7.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg7.TabIndex = 36;
            this.textBoxChip2Reg7.TextChanged += new System.EventHandler(this.textBoxChip2Reg7_TextChanged);
            this.textBoxChip2Reg7.Leave += new System.EventHandler(this.TextBoxExg2Reg7);
            // 
            // textBoxChip1Reg5
            // 
            this.textBoxChip1Reg5.Location = new System.Drawing.Point(195, 111);
            this.textBoxChip1Reg5.Name = "textBoxChip1Reg5";
            this.textBoxChip1Reg5.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg5.TabIndex = 23;
            this.textBoxChip1Reg5.TextChanged += new System.EventHandler(this.textBoxChip1Reg5_TextChanged);
            this.textBoxChip1Reg5.Leave += new System.EventHandler(this.TextBoxExg1Reg5_OnLeave);
            // 
            // textBoxChip2Reg8
            // 
            this.textBoxChip2Reg8.Location = new System.Drawing.Point(297, 146);
            this.textBoxChip2Reg8.Name = "textBoxChip2Reg8";
            this.textBoxChip2Reg8.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg8.TabIndex = 37;
            this.textBoxChip2Reg8.TextChanged += new System.EventHandler(this.textBoxChip2Reg8_TextChanged);
            this.textBoxChip2Reg8.Leave += new System.EventHandler(this.TextBoxExg2Reg8);
            // 
            // textBoxChip1Reg10
            // 
            this.textBoxChip1Reg10.Location = new System.Drawing.Point(365, 111);
            this.textBoxChip1Reg10.Name = "textBoxChip1Reg10";
            this.textBoxChip1Reg10.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip1Reg10.TabIndex = 28;
            this.textBoxChip1Reg10.TextChanged += new System.EventHandler(this.textBoxChip1Reg10_TextChanged);
            this.textBoxChip1Reg10.Leave += new System.EventHandler(this.TextBoxExg1Reg10_OnLeave);
            // 
            // textBoxChip2Reg9
            // 
            this.textBoxChip2Reg9.Location = new System.Drawing.Point(331, 146);
            this.textBoxChip2Reg9.Name = "textBoxChip2Reg9";
            this.textBoxChip2Reg9.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg9.TabIndex = 38;
            this.textBoxChip2Reg9.TextChanged += new System.EventHandler(this.textBoxChip2Reg9_TextChanged);
            this.textBoxChip2Reg9.Leave += new System.EventHandler(this.TextBoxExg2Reg9);
            // 
            // textBoxChip2Reg1
            // 
            this.textBoxChip2Reg1.Location = new System.Drawing.Point(59, 146);
            this.textBoxChip2Reg1.Name = "textBoxChip2Reg1";
            this.textBoxChip2Reg1.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg1.TabIndex = 30;
            this.textBoxChip2Reg1.TextChanged += new System.EventHandler(this.textBoxChip2Reg1_TextChanged);
            this.textBoxChip2Reg1.Leave += new System.EventHandler(this.TextBoxExg2Reg1_OnLeave);
            // 
            // textBoxChip2Reg10
            // 
            this.textBoxChip2Reg10.Location = new System.Drawing.Point(365, 146);
            this.textBoxChip2Reg10.Name = "textBoxChip2Reg10";
            this.textBoxChip2Reg10.Size = new System.Drawing.Size(24, 20);
            this.textBoxChip2Reg10.TabIndex = 39;
            this.textBoxChip2Reg10.TextChanged += new System.EventHandler(this.textBoxChip2Reg10_TextChanged);
            this.textBoxChip2Reg10.Leave += new System.EventHandler(this.TextBoxExg2Reg10);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.checkBoxDefaultExGTest);
            this.groupBoxSettings.Controls.Add(this.checkBoxDefaultEMG);
            this.groupBoxSettings.Controls.Add(this.checkBoxDefaultECG);
            this.groupBoxSettings.Controls.Add(this.labelEXG);
            this.groupBoxSettings.Controls.Add(this.label1);
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
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg6);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg3);
            this.groupBoxSettings.Controls.Add(this.labelChip1);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg2);
            this.groupBoxSettings.Controls.Add(this.textBoxChip1Reg1);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 136);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(481, 185);
            this.groupBoxSettings.TabIndex = 6;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "ExG Settings";
            this.groupBoxSettings.Enter += new System.EventHandler(this.groupBoxSettings_Enter);
            // 
            // checkBoxDefaultExGTest
            // 
            this.checkBoxDefaultExGTest.AutoSize = true;
            this.checkBoxDefaultExGTest.Location = new System.Drawing.Point(297, 35);
            this.checkBoxDefaultExGTest.Name = "checkBoxDefaultExGTest";
            this.checkBoxDefaultExGTest.Size = new System.Drawing.Size(107, 17);
            this.checkBoxDefaultExGTest.TabIndex = 44;
            this.checkBoxDefaultExGTest.Text = "Default ExG Test";
            this.checkBoxDefaultExGTest.UseVisualStyleBackColor = true;
            this.checkBoxDefaultExGTest.CheckedChanged += new System.EventHandler(this.checkBoxDefaultExGTest_CheckedChanged);
            this.checkBoxDefaultExGTest.Click += new System.EventHandler(this.checkBoxDefaultExGTest_Click);
            // 
            // checkBoxDefaultEMG
            // 
            this.checkBoxDefaultEMG.AutoSize = true;
            this.checkBoxDefaultEMG.Location = new System.Drawing.Point(161, 35);
            this.checkBoxDefaultEMG.Name = "checkBoxDefaultEMG";
            this.checkBoxDefaultEMG.Size = new System.Drawing.Size(87, 17);
            this.checkBoxDefaultEMG.TabIndex = 43;
            this.checkBoxDefaultEMG.Text = "Default EMG";
            this.checkBoxDefaultEMG.UseVisualStyleBackColor = true;
            this.checkBoxDefaultEMG.CheckedChanged += new System.EventHandler(this.checkBoxDefaultEMG_CheckedChanged);
            this.checkBoxDefaultEMG.Click += new System.EventHandler(this.checkBoxDefaultEMG_Click);
            // 
            // checkBoxDefaultECG
            // 
            this.checkBoxDefaultECG.AutoSize = true;
            this.checkBoxDefaultECG.Location = new System.Drawing.Point(16, 35);
            this.checkBoxDefaultECG.Name = "checkBoxDefaultECG";
            this.checkBoxDefaultECG.Size = new System.Drawing.Size(85, 17);
            this.checkBoxDefaultECG.TabIndex = 42;
            this.checkBoxDefaultECG.Text = "Default ECG";
            this.checkBoxDefaultECG.UseVisualStyleBackColor = true;
            this.checkBoxDefaultECG.CheckedChanged += new System.EventHandler(this.checkBoxDefaultECG_CheckedChanged);
            this.checkBoxDefaultECG.Click += new System.EventHandler(this.checkBoxDefaultECG_Click);
            // 
            // labelEXG
            // 
            this.labelEXG.AutoSize = true;
            this.labelEXG.Location = new System.Drawing.Point(68, 77);
            this.labelEXG.Name = "labelEXG";
            this.labelEXG.Size = new System.Drawing.Size(42, 13);
            this.labelEXG.TabIndex = 41;
            this.labelEXG.Text = "Custom";
            this.labelEXG.Click += new System.EventHandler(this.labelEXG_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Setting : ";
            // 
            // comboBoxExGReferenceElectrode
            // 
            this.comboBoxExGReferenceElectrode.FormattingEnabled = true;
            this.comboBoxExGReferenceElectrode.Location = new System.Drawing.Point(17, 351);
            this.comboBoxExGReferenceElectrode.Name = "comboBoxExGReferenceElectrode";
            this.comboBoxExGReferenceElectrode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxExGReferenceElectrode.TabIndex = 42;
            this.comboBoxExGReferenceElectrode.SelectedIndexChanged += new System.EventHandler(this.comboBoxExGReferenceElectrode_SelectedIndexChanged);
            this.comboBoxExGReferenceElectrode.SelectionChangeCommitted += new System.EventHandler(this.comboBoxExGReferenceElectrode_SelectionChangeCommitted);
            // 
            // labelExGReferenceElectrode
            // 
            this.labelExGReferenceElectrode.AutoSize = true;
            this.labelExGReferenceElectrode.Location = new System.Drawing.Point(15, 335);
            this.labelExGReferenceElectrode.Name = "labelExGReferenceElectrode";
            this.labelExGReferenceElectrode.Size = new System.Drawing.Size(128, 13);
            this.labelExGReferenceElectrode.TabIndex = 43;
            this.labelExGReferenceElectrode.Text = "ExG Reference Electrode";
            // 
            // comboBoxLeadOffDetection
            // 
            this.comboBoxLeadOffDetection.FormattingEnabled = true;
            this.comboBoxLeadOffDetection.Location = new System.Drawing.Point(18, 401);
            this.comboBoxLeadOffDetection.Name = "comboBoxLeadOffDetection";
            this.comboBoxLeadOffDetection.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLeadOffDetection.TabIndex = 44;
            this.comboBoxLeadOffDetection.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLeadOffDetection_SelectionChangeCommitted);
            // 
            // labelExGLeadOffDetection
            // 
            this.labelExGLeadOffDetection.AutoSize = true;
            this.labelExGLeadOffDetection.Location = new System.Drawing.Point(15, 385);
            this.labelExGLeadOffDetection.Name = "labelExGLeadOffDetection";
            this.labelExGLeadOffDetection.Size = new System.Drawing.Size(120, 13);
            this.labelExGLeadOffDetection.TabIndex = 45;
            this.labelExGLeadOffDetection.Text = "ExG Lead-Off Detection";
            // 
            // comboBoxExGLeadOffCurrent
            // 
            this.comboBoxExGLeadOffCurrent.FormattingEnabled = true;
            this.comboBoxExGLeadOffCurrent.Location = new System.Drawing.Point(173, 351);
            this.comboBoxExGLeadOffCurrent.Name = "comboBoxExGLeadOffCurrent";
            this.comboBoxExGLeadOffCurrent.Size = new System.Drawing.Size(121, 21);
            this.comboBoxExGLeadOffCurrent.TabIndex = 46;
            this.comboBoxExGLeadOffCurrent.SelectionChangeCommitted += new System.EventHandler(this.comboBoxExGLeadOffCurrent_SelectionChangeCommitted);
            // 
            // comboBoxLeadOffComparatorThreshold
            // 
            this.comboBoxLeadOffComparatorThreshold.FormattingEnabled = true;
            this.comboBoxLeadOffComparatorThreshold.Location = new System.Drawing.Point(173, 401);
            this.comboBoxLeadOffComparatorThreshold.Name = "comboBoxLeadOffComparatorThreshold";
            this.comboBoxLeadOffComparatorThreshold.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLeadOffComparatorThreshold.TabIndex = 47;
            this.comboBoxLeadOffComparatorThreshold.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLeadOffComparatorThreshold_SelectionChangeCommitted);
            // 
            // labelExGLeadOffCurrent
            // 
            this.labelExGLeadOffCurrent.AutoSize = true;
            this.labelExGLeadOffCurrent.Location = new System.Drawing.Point(170, 335);
            this.labelExGLeadOffCurrent.Name = "labelExGLeadOffCurrent";
            this.labelExGLeadOffCurrent.Size = new System.Drawing.Size(108, 13);
            this.labelExGLeadOffCurrent.TabIndex = 48;
            this.labelExGLeadOffCurrent.Text = "ExG Lead-Off Current";
            // 
            // labelExGLeadOffComparatorThreshold
            // 
            this.labelExGLeadOffComparatorThreshold.AutoSize = true;
            this.labelExGLeadOffComparatorThreshold.Location = new System.Drawing.Point(170, 385);
            this.labelExGLeadOffComparatorThreshold.Name = "labelExGLeadOffComparatorThreshold";
            this.labelExGLeadOffComparatorThreshold.Size = new System.Drawing.Size(178, 13);
            this.labelExGLeadOffComparatorThreshold.TabIndex = 49;
            this.labelExGLeadOffComparatorThreshold.Text = "ExG Lead-Off Comparator Threshold";
            // 
            // UserControlExgConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelExGLeadOffComparatorThreshold);
            this.Controls.Add(this.labelExGLeadOffCurrent);
            this.Controls.Add(this.comboBoxLeadOffComparatorThreshold);
            this.Controls.Add(this.comboBoxExGLeadOffCurrent);
            this.Controls.Add(this.labelExGLeadOffDetection);
            this.Controls.Add(this.comboBoxLeadOffDetection);
            this.Controls.Add(this.labelExGReferenceElectrode);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.comboBoxExGReferenceElectrode);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.groupBoxFilters);
            this.Name = "UserControlExgConfig";
            this.Size = new System.Drawing.Size(795, 440);
            this.Load += new System.EventHandler(this.UserControlExgConfig_Load);
            this.groupBoxFilters.ResumeLayout(false);
            this.groupBoxFilters.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.CheckBox checkBoxBSF60;
        private System.Windows.Forms.CheckBox checkBoxHPF0_05;
        private System.Windows.Forms.CheckBox checkBoxBSF50;
        private System.Windows.Forms.CheckBox checkBoxHPF0_5;
        private System.Windows.Forms.CheckBox checkBoxHPF5;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.CheckBox checkBoxNQFilter;
        protected internal System.Windows.Forms.TextBox textBoxChip1Reg1;
        private System.Windows.Forms.TextBox textBoxChip1Reg2;
        private System.Windows.Forms.Label labelChip1;
        private System.Windows.Forms.TextBox textBoxChip1Reg3;
        private System.Windows.Forms.TextBox textBoxChip1Reg6;
        private System.Windows.Forms.TextBox textBoxChip1Reg7;
        private System.Windows.Forms.TextBox textBoxChip2Reg2;
        private System.Windows.Forms.TextBox textBoxChip2Reg3;
        private System.Windows.Forms.TextBox textBoxChip1Reg4;
        private System.Windows.Forms.TextBox textBoxChip2Reg4;
        private System.Windows.Forms.TextBox textBoxChip1Reg8;
        private System.Windows.Forms.TextBox textBoxChip2Reg5;
        private System.Windows.Forms.Label labelChip2;
        private System.Windows.Forms.TextBox textBoxChip2Reg6;
        private System.Windows.Forms.TextBox textBoxChip1Reg9;
        private System.Windows.Forms.TextBox textBoxChip2Reg7;
        private System.Windows.Forms.TextBox textBoxChip1Reg5;
        private System.Windows.Forms.TextBox textBoxChip2Reg8;
        private System.Windows.Forms.TextBox textBoxChip1Reg10;
        private System.Windows.Forms.TextBox textBoxChip2Reg9;
        protected internal System.Windows.Forms.TextBox textBoxChip2Reg1;
        private System.Windows.Forms.TextBox textBoxChip2Reg10;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Label labelEXG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxExGReferenceElectrode;
        private System.Windows.Forms.Label labelExGReferenceElectrode;
        private System.Windows.Forms.Label labelExGLeadOffDetection;
        private System.Windows.Forms.ComboBox comboBoxExGLeadOffCurrent;
        private System.Windows.Forms.ComboBox comboBoxLeadOffComparatorThreshold;
        private System.Windows.Forms.Label labelExGLeadOffCurrent;
        private System.Windows.Forms.Label labelExGLeadOffComparatorThreshold;
        public System.Windows.Forms.ComboBox comboBoxLeadOffDetection;
        private System.Windows.Forms.CheckBox checkBoxDefaultExGTest;
        private System.Windows.Forms.CheckBox checkBoxDefaultEMG;
        private System.Windows.Forms.CheckBox checkBoxDefaultECG;
    }
}