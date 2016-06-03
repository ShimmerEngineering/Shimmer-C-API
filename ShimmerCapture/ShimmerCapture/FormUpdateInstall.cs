/*
 * C# Shimmer Update Install
 * 
 * Copyright (c) 2014, Shimmer Research, Ltd.
 * All rights reserved
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:

 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 *       copyright notice, this list of conditions and the following
 *       disclaimer in the documentation and/or other materials provided
 *       with the distribution.
 *     * Neither the name of Shimmer Research, Ltd. nor the names of its
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.

 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * @author Mark Nolan
 * @date   14th August, 2014
 * 
 */

//TODO: Add ability to cross-compare Checksum of downloaded update

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO; // to get file path

namespace ShimmerAPI
{
    public partial class FormUpdateInstall : Form
    {
        CheckBox ckBox;

        string[,] GlobalArray = null;

        public FormUpdateInstall(string[,] arr)
        {
            InitializeComponent();

            GlobalArray = arr;
            this.MinimumSize = new Size(this.Size.Width, this.Size.Height);
            this.MaximumSize = new Size(this.Size.Width, 800);
        }

        private int GridViewColIndxCheckbox = 0;


        private void FormUpdateInstall_Load(object sender, EventArgs e)
        {
            int GridViewColIndxUpdateType = 1;
            int GridViewColIndxName = 2;
            int GridViewColIndxVersion = 3;
            int GridViewColIndxDate = 4;

            //http://stackoverflow.com/questions/13242861/check-all-checkbox-items-on-datagridview
            DataGridViewCheckBoxColumn CheckboxColumn = new DataGridViewCheckBoxColumn();
            CheckboxColumn.Width = 20;
            dataGridViewUpdates.Columns.Add(CheckboxColumn);

            dataGridViewUpdates.Columns.Add("Type", "Type");
            dataGridViewUpdates.Columns.Add("Name", "Name");
            dataGridViewUpdates.Columns.Add("Version", "Version");
            dataGridViewUpdates.Columns.Add("Date", "Date");


            dataGridViewUpdates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUpdates.MultiSelect = true;
            //dataGridViewUpdates.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgv_RowPrePaint);
            dataGridViewUpdates.SelectAll();
            dataGridViewUpdates.SelectionChanged += new EventHandler(dataGridViewUpdates_SelectionChanged);
            dataGridViewUpdates.CellClick += new DataGridViewCellEventHandler(dataGridViewUpdates_CellClick);

            dataGridViewUpdates.Columns[GridViewColIndxCheckbox].Width = (int)(dataGridViewUpdates.Size.Width * 0.04);
            dataGridViewUpdates.Columns[GridViewColIndxUpdateType].Width = (int)(dataGridViewUpdates.Size.Width * 0.16);
            dataGridViewUpdates.Columns[GridViewColIndxName].Width = (int)(dataGridViewUpdates.Size.Width * 0.50);
            dataGridViewUpdates.Columns[GridViewColIndxVersion].Width = (int)(dataGridViewUpdates.Size.Width * 0.10);
            dataGridViewUpdates.Columns[GridViewColIndxDate].Width = (int)(dataGridViewUpdates.Size.Width * 0.14);

            dataGridViewUpdates.Columns[GridViewColIndxUpdateType].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewUpdates.Columns[GridViewColIndxName].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewUpdates.Columns[GridViewColIndxVersion].SortMode = DataGridViewColumnSortMode.Automatic;
            dataGridViewUpdates.Columns[GridViewColIndxDate].SortMode = DataGridViewColumnSortMode.Automatic;

            // Populate datagrid
            string[] DataRow = new string[dataGridViewUpdates.Columns.Count];
            for (int ii = 0; ii < GlobalArray.GetLength(0); ii++)
            {
                DataRow[GridViewColIndxUpdateType] = GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.category];
                DataRow[GridViewColIndxName] = GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.name];
                
                // Special case for Shimmer User Manual - will not work if file name changed from "*Shimmer User Manual*"
                if (GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.name].Contains("Shimmer User Manual"))
                {
                    DataRow[GridViewColIndxVersion] = "v" + GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.major] + GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.revision];
                }
                else
                {
                    DataRow[GridViewColIndxVersion] = "v" + GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.major] + "." + GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.minor] + "." + GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.revision];
                }

                string BufferMonth = GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.month];
                if (BufferMonth.Count() < 2)
                {
                    BufferMonth = "0" + BufferMonth;
                }
                string BufferDate = GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.day];
                if (BufferDate.Count() < 2)
                {
                    BufferDate = "0" + BufferDate;
                }

                DataRow[GridViewColIndxDate] = GlobalArray[ii, (int)FormUpdateCheck.NewsestVersionsColumnsOrder.year] + "-" + BufferMonth + "-" + BufferDate;

                dataGridViewUpdates.Rows.Add(DataRow);
            }


            // Set up one for all checkbox in column header
            //http://social.msdn.microsoft.com/Forums/windows/en-US/827907ea-c529-4254-9b15-2e6d571f5c5b/adding-a-checkbox-to-a-datagridview-column-header?forum=winformsdatacontrols
            ckBox = new CheckBox();
            //Get the column header cell bounds
            Rectangle rect = this.dataGridViewUpdates.GetCellDisplayRectangle(GridViewColIndxCheckbox, -1, true);
            ckBox.Size = new Size(17, 17);

            //Change the location of the CheckBox to make it stay on the header
            // x-coordinates increasing to the right
            // y-coordinates increasing from top to bottom
            ckBox.Location = new Point(rect.Left+3, rect.Top+3);

 //           ckBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
            ckBox.Click += new EventHandler(ckBox_Clicked);
            //Add the CheckBox into the DataGridView
            this.dataGridViewUpdates.Controls.Add(ckBox);

            ckBox.Checked = true;
            for (int j = 0; j < this.dataGridViewUpdates.RowCount; j++)
            {
                this.dataGridViewUpdates[GridViewColIndxCheckbox, j].Value = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerUpdatesInstall.IsBusy)
            {
                backgroundWorkerUpdatesInstall.CancelAsync();
            }

            this.Close();
        }

        private void dataGridViewUpdates_SelectionChanged(Object sender, EventArgs e)
        {
            dataGridViewUpdates.ClearSelection();
        }

        void ckBox_Clicked(object sender, EventArgs e)
        {
            dataGridViewUpdates.ClearSelection();
            this.dataGridViewUpdates.EndEdit();

            for (int j = 0; j < this.dataGridViewUpdates.RowCount; j++)
            {
                this.dataGridViewUpdates[GridViewColIndxCheckbox, j].Value = this.ckBox.Checked;
            }
        }

        private void dataGridViewUpdates_CellClick(Object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == GridViewColIndxCheckbox)
            {
                // Check if last value is true, if yes then assume checkbox must be changing to false -> set header checkbox to match
                if (this.dataGridViewUpdates[e.ColumnIndex, e.RowIndex].EditedFormattedValue.Equals(true))
                {
                    this.ckBox.Checked = false;

                    // Explicitly change the selected checkbox value (incase user clicks on edges around the tickbox)
                    dataGridViewUpdates.ClearSelection();
                    this.dataGridViewUpdates.EndEdit();
                    this.dataGridViewUpdates[e.ColumnIndex, e.RowIndex].Value = false;
                }
                else // Else check if all checkboxes enabled -> set header checkbox to match
                {
                    bool AllChecked = true;
                    for (int j = 0; j < this.dataGridViewUpdates.RowCount; j++)
                    {
                        // Skip changed checkbox as value will not have been updated yet
                        if (this.dataGridViewUpdates[GridViewColIndxCheckbox, j] != this.dataGridViewUpdates[e.ColumnIndex, e.RowIndex])
                        {
                            if (this.dataGridViewUpdates[GridViewColIndxCheckbox, j].EditedFormattedValue.Equals(false))
                            {
                                AllChecked = false;
                            }
                        }

                        if ((j == (this.dataGridViewUpdates.RowCount - 1)) && (AllChecked == true))
                        {
                            this.ckBox.Checked = true;
                        }
                    }

                    // Explicitly change the selected checkbox value (incase user clicks on edges around the tickbox)
                    dataGridViewUpdates.ClearSelection();
                    this.dataGridViewUpdates.EndEdit();
                    this.dataGridViewUpdates[e.ColumnIndex, e.RowIndex].Value = true;
                }
            }
        }

        //private void dgv_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        //{
        //    e.PaintParts &= ~DataGridViewPaintParts.Focus;
        //} //Only allows the user to select the entire row rather than single columns

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //http://msdn.microsoft.com/en-us/library/ms404280(v=vs.110).aspx - compression

            btnDownload.Text = "Downloading";
            btnDownload.Enabled = false;

            backgroundWorkerUpdatesInstall = new BackgroundWorker();
            backgroundWorkerUpdatesInstall.WorkerReportsProgress = true;
            backgroundWorkerUpdatesInstall.WorkerSupportsCancellation = true;
            backgroundWorkerUpdatesInstall.DoWork += backgroundWorker1_DoWork;
            backgroundWorkerUpdatesInstall.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorkerUpdatesInstall.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorkerUpdatesInstall.RunWorkerAsync();
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            bool error = false;
            //            for (int j = 0; j < this.dataGridViewUpdates.RowCount; j++)
            for (int j = this.dataGridViewUpdates.RowCount - 1; j >= 0; j--)
            {
                // check to see if thread was cancelled
                if (this.backgroundWorkerUpdatesInstall.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (this.dataGridViewUpdates[GridViewColIndxCheckbox, j].Value.Equals(true))
                {
                    string remoteUri = GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.URL];
                    string fileName = Path.GetFileName(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.URL]), myStringWebResource = null;

                    // Firmwware 
                    if (GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.category] == "Firmware")
                    {
                        // Create a new WebClient instance.
                        using (WebClient myWebClient = new WebClient())
                        {
                            myStringWebResource = remoteUri + fileName;
                            try
                            {
                                // Download the Web resource and save it into the current filesystem folder.
                                myWebClient.DownloadFile(remoteUri, FormUpdateCheck.FolderContainingFirmware + @"\" + fileName);

                                if ((this.chkbxOverwriteFW.Checked.Equals(true)) && (GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path] != null) && (File.Exists(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path])))
                                {
                                    File.Delete(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path]);
                                }

                                backgroundWorkerUpdatesInstall.ReportProgress(0,j);
                            }
                            catch
                            {
                                error = true;
                            }
                        }
                    }
                    // Software 
                    else if (GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.category] == "Software")
                    {
                        // Create a new WebClient instance.
                        using (WebClient myWebClient = new WebClient())
                        {
                            myStringWebResource = remoteUri + fileName;
                            try
                            {
                                // Download the Web resource and save it into the current filesystem folder.
                                myWebClient.DownloadFile(remoteUri, FormUpdateCheck.FolderContainingSoftware + @"\" + fileName);

                                if ((GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path] != null) && (File.Exists(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path])))
                                {
                                    File.Delete(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path]);
                                }

                                backgroundWorkerUpdatesInstall.ReportProgress(0, j);
                            }
                            catch
                            {
                                error = true;
                            }
                        }
                    }
                    // Documentation 
                    else if (GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.category] == "Documentation")
                    {
                        // Create a new WebClient instance.
                        using (WebClient myWebClient = new WebClient())
                        {
                            myStringWebResource = remoteUri + fileName;
                            try
                            {
                                // Download the Web resource and save it into the current filesystem folder.
                                myWebClient.DownloadFile(remoteUri, FormUpdateCheck.FolderContainingDocumentation + @"\" + fileName);

                                if ((GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path] != null) && (File.Exists(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path])))
                                {
                                    File.Delete(GlobalArray[j, (int)FormUpdateCheck.ListOfUpdatesColumnsOrder.path]);
                                }

                                backgroundWorkerUpdatesInstall.ReportProgress(0,j);
                            }
                            catch
                            {
                                error = true;
                            }
                        }
                    }
                }
            }

            e.Result = error;
        }

        // This event handler updates the UI
        private void backgroundWorker1_ProgressChanged(object sender,  ProgressChangedEventArgs e)
        {
            // Update the UI here
            dataGridViewUpdates.Rows.RemoveAt((int)e.UserState);
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // check to see if thread was cancelled
            if (e.Cancelled == true)
            {
                return;
            }

            if (e.Result.Equals(true))
            {
                string messageBoxText = "Updating failed.";
                string caption = "Bootstrap Loader";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(this, messageBoxText, caption, button, icon);
            }
            else
            {
                string messageBoxText = "Update successful.";
                string caption = "Bootstrap Loader";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.None;
                MessageBox.Show(this, messageBoxText, caption, button, icon);
                this.Close();
            }

            btnDownload.Text = "Download";
            btnDownload.Enabled = true;
        }

    }
}

//http://stackoverflow.com/questions/10520048/calculate-md5-checksum-for-a-file
//using (var md5 = MD5.Create())
//{
//    using (var stream = File.OpenRead(filename))
//    {
////return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
//        return md5.ComputeHash(stream);
//    }
//}
