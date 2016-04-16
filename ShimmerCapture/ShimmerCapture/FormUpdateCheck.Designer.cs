namespace ShimmerAPI
{
    partial class FormUpdateCheck
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInternetCheck = new System.Windows.Forms.Label();
            this.lblShimmerCheck = new System.Windows.Forms.Label();
            this.lblDownloading = new System.Windows.Forms.Label();
            this.picbxNetConnection = new System.Windows.Forms.PictureBox();
            this.picbxShimmerConnection = new System.Windows.Forms.PictureBox();
            this.picbxDLUpdates = new System.Windows.Forms.PictureBox();
            this.backgroundWorkerUpdateCheck = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.picbxNetConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbxShimmerConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbxDLUpdates)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(85, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInternetCheck
            // 
            this.lblInternetCheck.AutoSize = true;
            this.lblInternetCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInternetCheck.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblInternetCheck.Location = new System.Drawing.Point(12, 21);
            this.lblInternetCheck.Name = "lblInternetCheck";
            this.lblInternetCheck.Size = new System.Drawing.Size(187, 16);
            this.lblInternetCheck.TabIndex = 2;
            this.lblInternetCheck.Text = "Checking internet connection...";
            // 
            // lblShimmerCheck
            // 
            this.lblShimmerCheck.AutoSize = true;
            this.lblShimmerCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShimmerCheck.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblShimmerCheck.Location = new System.Drawing.Point(12, 50);
            this.lblShimmerCheck.Name = "lblShimmerCheck";
            this.lblShimmerCheck.Size = new System.Drawing.Size(186, 16);
            this.lblShimmerCheck.TabIndex = 2;
            this.lblShimmerCheck.Text = "Contacting Shimmer Servers...";
            // 
            // lblDownloading
            // 
            this.lblDownloading.AutoSize = true;
            this.lblDownloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloading.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblDownloading.Location = new System.Drawing.Point(12, 79);
            this.lblDownloading.Name = "lblDownloading";
            this.lblDownloading.Size = new System.Drawing.Size(188, 16);
            this.lblDownloading.TabIndex = 2;
            this.lblDownloading.Text = "Downloading List of Updates...";
            // 
            // picbxNetConnection
            // 
            this.picbxNetConnection.Location = new System.Drawing.Point(201, 18);
            this.picbxNetConnection.Name = "picbxNetConnection";
            this.picbxNetConnection.Size = new System.Drawing.Size(20, 20);
            this.picbxNetConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbxNetConnection.TabIndex = 3;
            this.picbxNetConnection.TabStop = false;
            // 
            // picbxShimmerConnection
            // 
            this.picbxShimmerConnection.Location = new System.Drawing.Point(201, 47);
            this.picbxShimmerConnection.Name = "picbxShimmerConnection";
            this.picbxShimmerConnection.Size = new System.Drawing.Size(20, 20);
            this.picbxShimmerConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbxShimmerConnection.TabIndex = 4;
            this.picbxShimmerConnection.TabStop = false;
            // 
            // picbxDLUpdates
            // 
            this.picbxDLUpdates.Location = new System.Drawing.Point(201, 76);
            this.picbxDLUpdates.Name = "picbxDLUpdates";
            this.picbxDLUpdates.Size = new System.Drawing.Size(20, 20);
            this.picbxDLUpdates.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbxDLUpdates.TabIndex = 5;
            this.picbxDLUpdates.TabStop = false;
            // 
            // backgroundWorkerUpdateCheck
            // 
            this.backgroundWorkerUpdateCheck.WorkerReportsProgress = true;
            // 
            // FormUpdateCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 150);
            this.Controls.Add(this.picbxDLUpdates);
            this.Controls.Add(this.picbxShimmerConnection);
            this.Controls.Add(this.picbxNetConnection);
            this.Controls.Add(this.lblDownloading);
            this.Controls.Add(this.lblShimmerCheck);
            this.Controls.Add(this.lblInternetCheck);
            this.Controls.Add(this.btnCancel);
            this.Icon = global::ShimmerAPI.Properties.Resources.ic_shimmercapture;
            this.Name = "FormUpdateCheck";
            this.Text = "Checking for updates";
            this.Load += new System.EventHandler(this.FormUpdateCheck_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbxNetConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbxShimmerConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbxDLUpdates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInternetCheck;
        private System.Windows.Forms.Label lblShimmerCheck;
        private System.Windows.Forms.Label lblDownloading;
        private System.Windows.Forms.PictureBox picbxNetConnection;
        private System.Windows.Forms.PictureBox picbxShimmerConnection;
        private System.Windows.Forms.PictureBox picbxDLUpdates;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUpdateCheck;

    }
}