namespace ShimmerAPI
{
    partial class Control
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control));
            this.buttonStreamandLog = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxShimmerState = new System.Windows.Forms.TextBox();
            this.labelState = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.ZedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.labelComPort = new System.Windows.Forms.Label();
            this.comboBoxComPorts = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripItemFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripItemTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSaveToCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemShow3DOrientation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonAddGraph = new System.Windows.Forms.Button();
            this.buttonRemoveGraph = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonStream = new System.Windows.Forms.Button();
            this.buttonReload = new System.Windows.Forms.Button();
            this.labelPRR = new System.Windows.Forms.Label();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.textBoxLeadOffStatus1 = new System.Windows.Forms.TextBox();
            this.textBoxLeadOffStatus3 = new System.Windows.Forms.TextBox();
            this.textBoxLeadOffStatus2 = new System.Windows.Forms.TextBox();
            this.textBoxLeadOffStatus4 = new System.Windows.Forms.TextBox();
            this.textBoxLeadOffStatus5 = new System.Windows.Forms.TextBox();
            this.labelLeadOffStatus2 = new System.Windows.Forms.Label();
            this.labelLeadOffStatus1 = new System.Windows.Forms.Label();
            this.labelLeadOffStatus5 = new System.Windows.Forms.Label();
            this.labelLeadOffStatus3 = new System.Windows.Forms.Label();
            this.labelLeadOffStatus4 = new System.Windows.Forms.Label();
            this.labelExGLeadOffDetection = new System.Windows.Forms.Label();
            this.buttonReadDirectory = new System.Windows.Forms.Button();
            this.buttonSetBlinkLED = new System.Windows.Forms.Button();
            this.checkBoxTSACheck = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxSawtoothValue = new System.Windows.Forms.TextBox();
            this.checkBoxCRC = new System.Windows.Forms.CheckBox();
            this.buttonStopStreamandLog = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonClearGraphs = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStreamandLog
            // 
            this.buttonStreamandLog.Enabled = false;
            this.buttonStreamandLog.Location = new System.Drawing.Point(252, 87);
            this.buttonStreamandLog.Margin = new System.Windows.Forms.Padding(5);
            this.buttonStreamandLog.Name = "buttonStreamandLog";
            this.buttonStreamandLog.Size = new System.Drawing.Size(165, 28);
            this.buttonStreamandLog.TabIndex = 2;
            this.buttonStreamandLog.Text = "Stream and Log";
            this.buttonStreamandLog.UseVisualStyleBackColor = true;
            this.buttonStreamandLog.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(41, 47);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(87, 28);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxShimmerState
            // 
            this.textBoxShimmerState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxShimmerState.Enabled = false;
            this.textBoxShimmerState.Location = new System.Drawing.Point(931, 92);
            this.textBoxShimmerState.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxShimmerState.Name = "textBoxShimmerState";
            this.textBoxShimmerState.Size = new System.Drawing.Size(133, 15);
            this.textBoxShimmerState.TabIndex = 122;
            this.textBoxShimmerState.Text = "None";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(819, 92);
            this.labelState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(97, 16);
            this.labelState.TabIndex = 121;
            this.labelState.Text = "Shimmer State:";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(684, 82);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(5);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(133, 34);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(684, 33);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(5);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(133, 34);
            this.buttonDisconnect.TabIndex = 4;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // ZedGraphControl1
            // 
            this.ZedGraphControl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ZedGraphControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ZedGraphControl1.Location = new System.Drawing.Point(40, 175);
            this.ZedGraphControl1.Margin = new System.Windows.Forms.Padding(5);
            this.ZedGraphControl1.Name = "ZedGraphControl1";
            this.ZedGraphControl1.ScrollGrace = 0D;
            this.ZedGraphControl1.ScrollMaxX = 0D;
            this.ZedGraphControl1.ScrollMaxY = 0D;
            this.ZedGraphControl1.ScrollMaxY2 = 0D;
            this.ZedGraphControl1.ScrollMinX = 0D;
            this.ZedGraphControl1.ScrollMinY = 0D;
            this.ZedGraphControl1.ScrollMinY2 = 0D;
            this.ZedGraphControl1.Size = new System.Drawing.Size(653, 380);
            this.ZedGraphControl1.TabIndex = 120;
            this.ZedGraphControl1.Load += new System.EventHandler(this.ZedGraphControl1_Load);
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Location = new System.Drawing.Point(848, 49);
            this.labelComPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(67, 16);
            this.labelComPort.TabIndex = 113;
            this.labelComPort.Text = "COM Port:";
            // 
            // comboBoxComPorts
            // 
            this.comboBoxComPorts.FormattingEnabled = true;
            this.comboBoxComPorts.Location = new System.Drawing.Point(931, 46);
            this.comboBoxComPorts.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxComPorts.Name = "comboBoxComPorts";
            this.comboBoxComPorts.Size = new System.Drawing.Size(125, 24);
            this.comboBoxComPorts.Sorted = true;
            this.comboBoxComPorts.TabIndex = 0;
            this.comboBoxComPorts.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPorts_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripItemFile,
            this.toolStripItemTools,
            this.toolStripSplitButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(2597, 27);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Text = "Check For Updates";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripItemFile
            // 
            this.toolStripItemFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemQuit});
            this.toolStripItemFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemFile.Name = "toolStripItemFile";
            this.toolStripItemFile.Size = new System.Drawing.Size(46, 24);
            this.toolStripItemFile.Text = "File";
            // 
            // ToolStripMenuItemQuit
            // 
            this.ToolStripMenuItemQuit.Name = "ToolStripMenuItemQuit";
            this.ToolStripMenuItemQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.ToolStripMenuItemQuit.ShowShortcutKeys = false;
            this.ToolStripMenuItemQuit.Size = new System.Drawing.Size(111, 26);
            this.ToolStripMenuItemQuit.Text = "Quit";
            this.ToolStripMenuItemQuit.Click += new System.EventHandler(this.ToolStripMenuItemQuit_Click);
            // 
            // toolStripItemTools
            // 
            this.toolStripItemTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.ToolStripMenuItemSaveToCSV,
            this.ToolStripMenuItemShow3DOrientation});
            this.toolStripItemTools.Image = ((System.Drawing.Image)(resources.GetObject("toolStripItemTools.Image")));
            this.toolStripItemTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripItemTools.Name = "toolStripItemTools";
            this.toolStripItemTools.Size = new System.Drawing.Size(58, 24);
            this.toolStripItemTools.Text = "Tools";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Enabled = false;
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(230, 26);
            this.configureToolStripMenuItem.Text = "Configuration";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemSaveToCSV
            // 
            this.ToolStripMenuItemSaveToCSV.Name = "ToolStripMenuItemSaveToCSV";
            this.ToolStripMenuItemSaveToCSV.Size = new System.Drawing.Size(230, 26);
            this.ToolStripMenuItemSaveToCSV.Text = "Save To CSV";
            this.ToolStripMenuItemSaveToCSV.Click += new System.EventHandler(this.ToolStripMenuItemSaveToCSV_Click);
            // 
            // ToolStripMenuItemShow3DOrientation
            // 
            this.ToolStripMenuItemShow3DOrientation.Enabled = false;
            this.ToolStripMenuItemShow3DOrientation.Name = "ToolStripMenuItemShow3DOrientation";
            this.ToolStripMenuItemShow3DOrientation.Size = new System.Drawing.Size(230, 26);
            this.ToolStripMenuItemShow3DOrientation.Text = "Show 3D Orientation";
            this.ToolStripMenuItemShow3DOrientation.Click += new System.EventHandler(this.ToolStripMenuItemShow3DOrientation_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(55, 24);
            this.toolStripSplitButton1.Text = "Help";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openDialog
            // 
            this.openDialog.DefaultExt = "csv";
            this.openDialog.FileName = "ShimmerData.csv";
            this.openDialog.Filter = "csv files|*.csv| All files|*.*";
            // 
            // buttonAddGraph
            // 
            this.buttonAddGraph.Location = new System.Drawing.Point(132, 140);
            this.buttonAddGraph.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddGraph.Name = "buttonAddGraph";
            this.buttonAddGraph.Size = new System.Drawing.Size(44, 28);
            this.buttonAddGraph.TabIndex = 112;
            this.buttonAddGraph.Text = "+";
            this.buttonAddGraph.UseVisualStyleBackColor = true;
            this.buttonAddGraph.Click += new System.EventHandler(this.buttonAddGraph_Click);
            // 
            // buttonRemoveGraph
            // 
            this.buttonRemoveGraph.Location = new System.Drawing.Point(341, 140);
            this.buttonRemoveGraph.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemoveGraph.Name = "buttonRemoveGraph";
            this.buttonRemoveGraph.Size = new System.Drawing.Size(44, 28);
            this.buttonRemoveGraph.TabIndex = 111;
            this.buttonRemoveGraph.Text = "-";
            this.buttonRemoveGraph.UseVisualStyleBackColor = true;
            this.buttonRemoveGraph.Click += new System.EventHandler(this.buttonRemoveGraph_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 145);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 78;
            this.label1.Text = "Add Graph";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 146);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 16);
            this.label2.TabIndex = 79;
            this.label2.Text = "Remove Graph";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1388);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 25, 0);
            this.statusStrip1.Size = new System.Drawing.Size(2597, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatusLabel
            // 
            this.tsStatusLabel.Name = "tsStatusLabel";
            this.tsStatusLabel.Size = new System.Drawing.Size(151, 20);
            this.tsStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // buttonStream
            // 
            this.buttonStream.Enabled = false;
            this.buttonStream.Location = new System.Drawing.Point(41, 87);
            this.buttonStream.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStream.Name = "buttonStream";
            this.buttonStream.Size = new System.Drawing.Size(165, 28);
            this.buttonStream.TabIndex = 123;
            this.buttonStream.Text = "Stream";
            this.buttonStream.UseVisualStyleBackColor = true;
            this.buttonStream.Click += new System.EventHandler(this.buttonStream_Click);
            // 
            // buttonReload
            // 
            this.buttonReload.Enabled = false;
            this.buttonReload.Location = new System.Drawing.Point(1065, 44);
            this.buttonReload.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(109, 28);
            this.buttonReload.TabIndex = 124;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // labelPRR
            // 
            this.labelPRR.AutoSize = true;
            this.labelPRR.Location = new System.Drawing.Point(1072, 92);
            this.labelPRR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPRR.Name = "labelPRR";
            this.labelPRR.Size = new System.Drawing.Size(149, 16);
            this.labelPRR.TabIndex = 125;
            this.labelPRR.Text = "Packet Reception Rate:";
            this.labelPRR.Click += new System.EventHandler(this.labelPRR_Click);
            // 
            // textBoxLeadOffStatus1
            // 
            this.textBoxLeadOffStatus1.Location = new System.Drawing.Point(1379, 82);
            this.textBoxLeadOffStatus1.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeadOffStatus1.Name = "textBoxLeadOffStatus1";
            this.textBoxLeadOffStatus1.Size = new System.Drawing.Size(45, 22);
            this.textBoxLeadOffStatus1.TabIndex = 136;
            // 
            // textBoxLeadOffStatus3
            // 
            this.textBoxLeadOffStatus3.Location = new System.Drawing.Point(1488, 82);
            this.textBoxLeadOffStatus3.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeadOffStatus3.Name = "textBoxLeadOffStatus3";
            this.textBoxLeadOffStatus3.Size = new System.Drawing.Size(45, 22);
            this.textBoxLeadOffStatus3.TabIndex = 137;
            // 
            // textBoxLeadOffStatus2
            // 
            this.textBoxLeadOffStatus2.Location = new System.Drawing.Point(1433, 82);
            this.textBoxLeadOffStatus2.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeadOffStatus2.Name = "textBoxLeadOffStatus2";
            this.textBoxLeadOffStatus2.Size = new System.Drawing.Size(45, 22);
            this.textBoxLeadOffStatus2.TabIndex = 138;
            // 
            // textBoxLeadOffStatus4
            // 
            this.textBoxLeadOffStatus4.Location = new System.Drawing.Point(1543, 82);
            this.textBoxLeadOffStatus4.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeadOffStatus4.Name = "textBoxLeadOffStatus4";
            this.textBoxLeadOffStatus4.Size = new System.Drawing.Size(45, 22);
            this.textBoxLeadOffStatus4.TabIndex = 139;
            // 
            // textBoxLeadOffStatus5
            // 
            this.textBoxLeadOffStatus5.Location = new System.Drawing.Point(1597, 81);
            this.textBoxLeadOffStatus5.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeadOffStatus5.Name = "textBoxLeadOffStatus5";
            this.textBoxLeadOffStatus5.Size = new System.Drawing.Size(45, 22);
            this.textBoxLeadOffStatus5.TabIndex = 140;
            // 
            // labelLeadOffStatus2
            // 
            this.labelLeadOffStatus2.AutoSize = true;
            this.labelLeadOffStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeadOffStatus2.Location = new System.Drawing.Point(1431, 68);
            this.labelLeadOffStatus2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeadOffStatus2.Name = "labelLeadOffStatus2";
            this.labelLeadOffStatus2.Size = new System.Drawing.Size(21, 13);
            this.labelLeadOffStatus2.TabIndex = 141;
            this.labelLeadOffStatus2.Text = "RA";
            // 
            // labelLeadOffStatus1
            // 
            this.labelLeadOffStatus1.AutoSize = true;
            this.labelLeadOffStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeadOffStatus1.Location = new System.Drawing.Point(1376, 68);
            this.labelLeadOffStatus1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeadOffStatus1.Name = "labelLeadOffStatus1";
            this.labelLeadOffStatus1.Size = new System.Drawing.Size(20, 13);
            this.labelLeadOffStatus1.TabIndex = 142;
            this.labelLeadOffStatus1.Text = "LA";
            // 
            // labelLeadOffStatus5
            // 
            this.labelLeadOffStatus5.AutoSize = true;
            this.labelLeadOffStatus5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeadOffStatus5.Location = new System.Drawing.Point(1595, 68);
            this.labelLeadOffStatus5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeadOffStatus5.Name = "labelLeadOffStatus5";
            this.labelLeadOffStatus5.Size = new System.Drawing.Size(28, 13);
            this.labelLeadOffStatus5.TabIndex = 143;
            this.labelLeadOffStatus5.Text = "RLD";
            // 
            // labelLeadOffStatus3
            // 
            this.labelLeadOffStatus3.AutoSize = true;
            this.labelLeadOffStatus3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeadOffStatus3.Location = new System.Drawing.Point(1485, 68);
            this.labelLeadOffStatus3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeadOffStatus3.Name = "labelLeadOffStatus3";
            this.labelLeadOffStatus3.Size = new System.Drawing.Size(19, 13);
            this.labelLeadOffStatus3.TabIndex = 144;
            this.labelLeadOffStatus3.Text = "LL";
            // 
            // labelLeadOffStatus4
            // 
            this.labelLeadOffStatus4.AutoSize = true;
            this.labelLeadOffStatus4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeadOffStatus4.Location = new System.Drawing.Point(1540, 68);
            this.labelLeadOffStatus4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeadOffStatus4.Name = "labelLeadOffStatus4";
            this.labelLeadOffStatus4.Size = new System.Drawing.Size(20, 13);
            this.labelLeadOffStatus4.TabIndex = 145;
            this.labelLeadOffStatus4.Text = "V1";
            // 
            // labelExGLeadOffDetection
            // 
            this.labelExGLeadOffDetection.AutoSize = true;
            this.labelExGLeadOffDetection.Location = new System.Drawing.Point(1375, 38);
            this.labelExGLeadOffDetection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExGLeadOffDetection.Name = "labelExGLeadOffDetection";
            this.labelExGLeadOffDetection.Size = new System.Drawing.Size(146, 16);
            this.labelExGLeadOffDetection.TabIndex = 146;
            this.labelExGLeadOffDetection.Text = "ExG Lead-Off Detection";
            // 
            // buttonReadDirectory
            // 
            this.buttonReadDirectory.Location = new System.Drawing.Point(1183, 44);
            this.buttonReadDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReadDirectory.Name = "buttonReadDirectory";
            this.buttonReadDirectory.Size = new System.Drawing.Size(165, 28);
            this.buttonReadDirectory.TabIndex = 147;
            this.buttonReadDirectory.Text = "Read Directory";
            this.buttonReadDirectory.UseVisualStyleBackColor = true;
            this.buttonReadDirectory.Click += new System.EventHandler(this.buttonReadDirectory_Click);
            // 
            // buttonSetBlinkLED
            // 
            this.buttonSetBlinkLED.Location = new System.Drawing.Point(252, 43);
            this.buttonSetBlinkLED.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSetBlinkLED.Name = "buttonSetBlinkLED";
            this.buttonSetBlinkLED.Size = new System.Drawing.Size(165, 31);
            this.buttonSetBlinkLED.TabIndex = 148;
            this.buttonSetBlinkLED.Text = "Set Blink LED";
            this.buttonSetBlinkLED.UseVisualStyleBackColor = true;
            this.buttonSetBlinkLED.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxTSACheck
            // 
            this.checkBoxTSACheck.AutoSize = true;
            this.checkBoxTSACheck.Location = new System.Drawing.Point(1676, 47);
            this.checkBoxTSACheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxTSACheck.Name = "checkBoxTSACheck";
            this.checkBoxTSACheck.Size = new System.Drawing.Size(205, 20);
            this.checkBoxTSACheck.TabIndex = 149;
            this.checkBoxTSACheck.Text = "Time Stamp Alignment Check";
            this.toolTip1.SetToolTip(this.checkBoxTSACheck, resources.GetString("checkBoxTSACheck.ToolTip"));
            this.checkBoxTSACheck.UseVisualStyleBackColor = true;
            this.checkBoxTSACheck.CheckedChanged += new System.EventHandler(this.checkBoxTSACheck_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 2000;
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 400;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1676, 76);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 26);
            this.button1.TabIndex = 150;
            this.button1.Text = "Reset PPGtoHR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(684, 124);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 41);
            this.button2.TabIndex = 151;
            this.button2.Text = "EXG Saw";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxSawtoothValue
            // 
            this.textBoxSawtoothValue.Location = new System.Drawing.Point(1233, 89);
            this.textBoxSawtoothValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxSawtoothValue.Name = "textBoxSawtoothValue";
            this.textBoxSawtoothValue.Size = new System.Drawing.Size(113, 22);
            this.textBoxSawtoothValue.TabIndex = 152;
            this.textBoxSawtoothValue.Text = "5000";
            this.textBoxSawtoothValue.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // checkBoxCRC
            // 
            this.checkBoxCRC.AutoSize = true;
            this.checkBoxCRC.Location = new System.Drawing.Point(132, 53);
            this.checkBoxCRC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxCRC.Name = "checkBoxCRC";
            this.checkBoxCRC.Size = new System.Drawing.Size(103, 20);
            this.checkBoxCRC.TabIndex = 153;
            this.checkBoxCRC.Text = "Enable CRC";
            this.checkBoxCRC.UseVisualStyleBackColor = true;
            this.checkBoxCRC.Visible = false;
            this.checkBoxCRC.CheckedChanged += new System.EventHandler(this.checkBox91_CheckedChanged);
            // 
            // buttonStopStreamandLog
            // 
            this.buttonStopStreamandLog.Enabled = false;
            this.buttonStopStreamandLog.Location = new System.Drawing.Point(447, 85);
            this.buttonStopStreamandLog.Margin = new System.Windows.Forms.Padding(5);
            this.buttonStopStreamandLog.Name = "buttonStopStreamandLog";
            this.buttonStopStreamandLog.Size = new System.Drawing.Size(208, 34);
            this.buttonStopStreamandLog.TabIndex = 154;
            this.buttonStopStreamandLog.Text = "Stop Stream and Log";
            this.buttonStopStreamandLog.UseVisualStyleBackColor = true;
            this.buttonStopStreamandLog.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(703, 175);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 380);
            this.panel1.TabIndex = 155;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(703, 570);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(413, 363);
            this.panel2.TabIndex = 156;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Location = new System.Drawing.Point(703, 948);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(413, 363);
            this.panel3.TabIndex = 157;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 146);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 158;
            this.label3.Text = "Clear Graphs";
            // 
            // buttonClearGraphs
            // 
            this.buttonClearGraphs.Location = new System.Drawing.Point(551, 140);
            this.buttonClearGraphs.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearGraphs.Name = "buttonClearGraphs";
            this.buttonClearGraphs.Size = new System.Drawing.Size(44, 28);
            this.buttonClearGraphs.TabIndex = 159;
            this.buttonClearGraphs.Text = "x";
            this.buttonClearGraphs.UseVisualStyleBackColor = true;
            this.buttonClearGraphs.Click += new System.EventHandler(this.buttonClearGraphs_Click_1);
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(2597, 1414);
            this.Controls.Add(this.buttonClearGraphs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonStopStreamandLog);
            this.Controls.Add(this.checkBoxCRC);
            this.Controls.Add(this.textBoxSawtoothValue);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxTSACheck);
            this.Controls.Add(this.buttonSetBlinkLED);
            this.Controls.Add(this.buttonReadDirectory);
            this.Controls.Add(this.labelExGLeadOffDetection);
            this.Controls.Add(this.labelLeadOffStatus4);
            this.Controls.Add(this.labelLeadOffStatus3);
            this.Controls.Add(this.labelLeadOffStatus5);
            this.Controls.Add(this.labelLeadOffStatus1);
            this.Controls.Add(this.labelLeadOffStatus2);
            this.Controls.Add(this.textBoxLeadOffStatus5);
            this.Controls.Add(this.textBoxLeadOffStatus4);
            this.Controls.Add(this.textBoxLeadOffStatus2);
            this.Controls.Add(this.textBoxLeadOffStatus3);
            this.Controls.Add(this.textBoxLeadOffStatus1);
            this.Controls.Add(this.labelPRR);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.buttonStream);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRemoveGraph);
            this.Controls.Add(this.buttonAddGraph);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.comboBoxComPorts);
            this.Controls.Add(this.labelComPort);
            this.Controls.Add(this.ZedGraphControl1);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.textBoxShimmerState);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonStreamandLog);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Control";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.buttonDisconnect_Click);
            this.Load += new System.EventHandler(this.ControlForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button buttonStreamandLog;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxShimmerState;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonDisconnect;
        private ZedGraph.ZedGraphControl ZedGraphControl1;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.ComboBox comboBoxComPorts;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripItemFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemQuit;
        private System.Windows.Forms.ToolStripDropDownButton toolStripItemTools;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSaveToCSV;
        public System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemShow3DOrientation;
        private System.Windows.Forms.Button buttonAddGraph;
        private System.Windows.Forms.Button buttonRemoveGraph;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
        private System.Windows.Forms.Button buttonStream;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.Label labelPRR;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.TextBox textBoxLeadOffStatus1;
        private System.Windows.Forms.TextBox textBoxLeadOffStatus3;
        private System.Windows.Forms.TextBox textBoxLeadOffStatus2;
        private System.Windows.Forms.TextBox textBoxLeadOffStatus4;
        private System.Windows.Forms.TextBox textBoxLeadOffStatus5;
        private System.Windows.Forms.Label labelLeadOffStatus2;
        private System.Windows.Forms.Label labelLeadOffStatus1;
        private System.Windows.Forms.Label labelLeadOffStatus5;
        private System.Windows.Forms.Label labelLeadOffStatus3;
        private System.Windows.Forms.Label labelLeadOffStatus4;
        private System.Windows.Forms.Label labelExGLeadOffDetection;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button buttonReadDirectory;
        private System.Windows.Forms.Button buttonSetBlinkLED;
        private System.Windows.Forms.CheckBox checkBoxTSACheck;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxSawtoothValue;
        private System.Windows.Forms.CheckBox checkBoxCRC;
        private System.Windows.Forms.Button buttonStopStreamandLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonClearGraphs;
    }
}

