/*
 * C# Shimmer Update Check
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

//TODO: Add ability for ShimmerCapture/Connect to indicate if currently used FW is up to date

/*
// All possible UpdatesToCheck entries.
// {UID, Search Description, Need to visit website to download}
// Firmware
{"0.0", "BtStream", "0"},  // Shimmer3 - Firmware - BtStream
{"0.1", "BtStream_ZIP", "0"},  // Shimmer3 - Firmware - BtStream
{"0.2", "SDLog", "0"},  // Shimmer3 - Firmware - SDLog
{"0.3", "SDLog_ZIP", "0"},  // Shimmer3 - Firmware - SDLog
{"0.4", "LogAndStream", "0"},  // Shimmer3 - Firmware - LogAndStream
{"0.5", "LogAndStream_ZIP", "0"},  // Shimmer3 - Firmware - LogAndStream
{"0.6", "Green_LED_Shimmer3", "0"},  // Shimmer3 - Firmware - Green LED Blink
{"0.7", "Red_LED_Shimmer3", "0"},  // Shimmer3 - Firmware - Red LED Blink
{"0.8", "Sleep_Shimmer3", "0"},  // Shimmer3 - Firmware - Shimmer3 Sleep

// Software
{"1.0", "", "1"},  // Shimmer3 - Software - ShimmerCapture
{"1.1", "", "1"},  // Shimmer3 - Software - Multi Shimmer Sync For Android
{"1.2", "", "1"},  // Shimmer3 - Software - Multi Shimmer Sync For SD
{"1.3", "", "1"},  // Shimmer3 - Software - Shimmer Bootstrap Loader (BSL)
{"1.4", "", "1"},  // Shimmer3 - Software - ShimmerLog
{"1.5", "", "1"},  // Shimmer3 - Software - Shimmer Android API
{"1.6", "", "1"},  // Shimmer3 - Software - Shimmer Matlab Instrument Driver
{"1.7", "", "1"},  // Shimmer3 - Software - ShimmerSensing LabVIEW Instrument Driver Library
{"1.8", "", "1"},  // Shimmer3 - Software - ShimmerConnect
{"1.9", "", "1"},  // Shimmer3 - Software - Shimmer 9DOF Calibration
{"1.10", "", "1"},  // Shimmer3 - Software - Shimmer C# API
{"1.11","Scripter", "0"},  // Shimmer3 - Software - Shimmer Bootstrap Loader Scripter
{"1.12", "BootstrapLoader", "1"},  // Shimmer3 - Software - Shimmer Bootstrap Loader Application

// Documentation
{"2.0", "BtStream", "0"},  // Shimmer3 - Documentation - BtStream for Shimmer3 Firmware User Manual
{"2.1", "SDLog", "0"},  // Shimmer3 - Documentation - SDLog for Shimmer3 Firmware User Manual
{"2.2", "LogAndStream", "0"},  // Shimmer3 - Documentation - LogAndStream for Shimmer3 Firmware User Manual
{"2.3", "", "0"},  // Shimmer3 - Documentation - ExG User Guide for ECG
{"2.4", "", "0"},  // Shimmer3 - Documentation - ExG User Guide for EMG
{"2.5", "", "0"},  // Shimmer3 - Documentation - Expansion Board User Guide
{"2.6", "", "0"},  // Shimmer3 - Documentation - IMU User Guide
{"2.7", "Shimmer_Dock", "0"},  // Shimmer3 - Documentation - Shimmer Dock User Guide
{"2.8", "", "0"},  // Shimmer3 - Documentation - Shimmer GSR+ Expansion Board User Guide
{"2.9", "", "0"},  // Shimmer3 - Documentation - Shimmer Multi Charger User Guide
{"2.10", "", "0"},  // Shimmer3 - Documentation - Shimmer Optical Pulse Sensing Probe User Guide
{"2.11", "", "0"},  // Shimmer3 - Documentation - Shimmer PROTO3 Series User Guide
{"2.12", "", "0"},  // Shimmer3 - Documentation - Shimmer3 Bridge Amplifier User Guide
{"2.13", "", "0"},  // Shimmer3 - Documentation - Shimmer3 JTAG Developer Board User Guide
{"2.14", "Shimmer_User_Manual", "0"},  // Shimmer3 - Documentation - Shimmer User Manual
{"2.15", "", "0"},  // Shimmer3 - Documentation - Multi Shimmer Sync for Android User Manual
{"2.16", "", "0"},  // Shimmer3 - Documentation - Multi Shimmer Sync for SD User Manual
{"2.17", "", "0"},  // Shimmer3 - Documentation - Multi Shimmer Sync for Windows User Manual
{"2.18", "", "0"},  // Shimmer3 - Documentation - Shimmer 9DOF Calibration User Manual
{"2.19", "", "0"},  // Shimmer3 - Documentation - Shimmer Connect User Manual
{"2.20", "", "0"},  // Shimmer3 - Documentation - Shimmer Sedentary PPG-to-HR Application User Manual
{"2.21", "", "0"},  // Shimmer3 - Documentation - ShimmerCapture User Manual
{"2.22", "", "0"},  // Shimmer3 - Documentation - ShimmerLog User Manual
{"2.23", "", "0"},  // Shimmer3 - Documentation - Shimmer Android Instrument Driver User Manual
{"2.24", "", "0"},  // Shimmer3 - Documentation - Shimmer Matlab Instrument Driver User Manual
{"2.25", "", "0"},  // Shimmer3 - Documentation - ShimmerSensing LabVIEW Library User Manual 
{"2.26", "", "0"},  // Shimmer3 - Documentation - Shimmer Plot For Android User Manual
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.IO; // For file operations

namespace ShimmerAPI
{
    public partial class FormUpdateCheck : Form
    {
        // Application Specific constants
        public static string ThisAppUID = string.Empty;
        public static string versionNumber = versionNumber = Application.ProductVersion.ToString(); // Application version number taken from "AssemblyInfo.cs"

        public static string FolderContainingFirmware = @"Bootstraps\Shimmer3 Samples";
        public static string FolderContainingSoftware = "Bin";
        public static string FolderContainingDocumentation = "Helpfiles";

        public string[,] UpdatesToCheck = null;

        List<String[,]> UpdatesList = new List<String[,]>();

        public enum Websites 
        {
            Google,
            Baidu,
            Shimmer
        }

        public enum NewsestVersionsColumnsOrder
        {
            UID,
            device,
            category,
            subcategory,
            name,
            major,
            minor,
            revision,
            day,
            month,
            year,
            URL,
            checksum,
            ArraySize //only used to set the array size
            //redunancy1,
            //redunancy2,
            //redunancy3
        }

        public enum CurrentVersionsColumnsOrder
        {
            UID,
            major,
            minor,
            revision,
            path,
            ArraySize //only used to set the array size
        }

        public enum ListOfUpdatesColumnsOrder
        {
            UID,
            device,
            category,
            subcategory,
            name,
            major,
            minor,
            revision,
            day,
            month,
            year,
            URL,
            checksum,
            path,
            webinstall,
            ArraySize //only used to set the array size
        }

        public FormUpdateCheck()
        {
            InitializeComponent();
            this.MinimumSize = new Size(this.Size.Width, this.Size.Height);
            this.MaximumSize = new Size(this.Size.Width, this.Size.Height);
        }

        private void FormUpdateCheck_Load(object sender, System.EventArgs e)
        {
            this.FormClosed += FormUpdateCheck_Closed;

            LoadApplicationSpecificContants();

            //http://www.nullskull.com/a/763/tools-for-updating-windows-forms-ui-from-background-threads.aspx
            backgroundWorkerUpdateCheck = new BackgroundWorker();
            backgroundWorkerUpdateCheck.WorkerReportsProgress = true;
            backgroundWorkerUpdateCheck.WorkerSupportsCancellation = true;
            backgroundWorkerUpdateCheck.DoWork += backgroundWorkerUpdateCheck_DoWork;
            backgroundWorkerUpdateCheck.RunWorkerCompleted += backgroundWorkerUpdateCheck_RunWorkerCompleted;

            backgroundWorkerUpdateCheck.RunWorkerAsync();
        }

        private void FormUpdateCheck_Closed(object sender, System.EventArgs e)
        {
            if (backgroundWorkerUpdateCheck.IsBusy.Equals(true))
            {
                backgroundWorkerUpdateCheck.CancelAsync();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadApplicationSpecificContants()
        {
            if (Application.ProductName.ToString().Contains("Shimmer3_Bootstrap_Loader"))
            {
                ThisAppUID = "1.0";

                UpdatesToCheck = new string[,]
                {
                    // Firmware
                    /*
                    {"0.0", "BtStream", "0"},  // Shimmer3 - Firmware - BtStream
                    {"0.2", "SDLog", "0"},  // Shimmer3 - Firmware - SDLog
                    {"0.4", "LogAndStream", "0"},  // Shimmer3 - Firmware - LogAndStream
                    {"0.6", "Green_LED", "0"},  // Shimmer3 - Firmware - Green LED Blink
                    {"0.7", "Red_LED", "0"},  // Shimmer3 - Firmware - Red LED Blink
                    {"0.8", "Sleep", "0"},  // Shimmer3 - Firmware - Shimmer3 Sleep
                    */
                    // Software
                    {"1.0", "", "1"},  // Shimmer3 - Software - ShimmerCapture
                    
                    /*
                    {"1.11","Scripter", "0"},  // Shimmer3 - Software - Shimmer Bootstrap Loader Scripter

                    // Documentation
                    {"2.0", "BtStream", "0"},  // Shimmer3 - Documentation - BtStream for Shimmer3 Firmware User Manual
                    {"2.1", "SDLog", "0"},  // Shimmer3 - Documentation - SDLog for Shimmer3 Firmware User Manual
                    {"2.2", "LogAndStream", "0"},  // Shimmer3 - Documentation - LogAndStream for Shimmer3 Firmware User Manual
                    {"2.7", "Shimmer_Dock", "0"},  // Shimmer3 - Documentation - Shimmer Dock User Guide
                    {"2.14", "Shimmer_User_Manual", "0"}  // Shimmer3 - Documentation - Shimmer User Manual
                    */ 
                };
            }
            else if (Application.ProductName.ToString().Contains("ShimmerCapture"))
            {
                ThisAppUID = "1.0";

                UpdatesToCheck = new string[,]
                {
                    // Firmware
                    //{"0.0", "BtStream", "0"},  // Shimmer3 - Firmware - BtStream
                    //{"0.2", "SDLog", "0"},  // Shimmer3 - Firmware - SDLog
                    //{"0.4", "LogAndStream", "0"},  // Shimmer3 - Firmware - LogAndStream
                    // Software
                    {"1.0", "", "1"},  // Shimmer3 - Software - ShimmerCapture
                    // Documentation
                    //{"2.21", "", "0"},  // Shimmer3 - Documentation - ShimmerCapture User Manual
                };

            }
            else if (Application.ProductName.ToString().Contains("ShimmerConnect"))
            {
                ThisAppUID = "1.8";

                UpdatesToCheck = new string[,]
                {
                    // Firmware
                    //{"0.0", "BtStream", "0"},  // Shimmer3 - Firmware - BtStream
                    //{"0.2", "SDLog", "0"},  // Shimmer3 - Firmware - SDLog
                    //{"0.4", "LogAndStream", "0"},  // Shimmer3 - Firmware - LogAndStream
                    // Software
                    {"1.8", "", "1"},  // Shimmer3 - Software - ShimmerConnect
                    // Documentation
                    {"2.19", "", "0"},  // Shimmer3 - Documentation - Shimmer Connect User Manual
                };

            }
            else
            {
                this.Close();
            }
        }

        private void backgroundWorkerUpdateCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            bool error = false;

            lblInternetCheck.ForeColor = Color.Black;
            // http://ajaxload.info/
            picbxNetConnection.Image = ShimmerAPI.Properties.Resources.inprogress;

            if (!CheckForInternetConnection(Websites.Google))
            {
                if (!CheckForInternetConnection(Websites.Baidu))
                {
                    picbxNetConnection.Image = ShimmerAPI.Properties.Resources.Tick_red;
                    string messageBoxText = "No internet connection detected.";
                    string caption = "Bootstrap Loader";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                    error = true;
                }
            }
            else
            {
                picbxNetConnection.Image = ShimmerAPI.Properties.Resources.Tick_green;
            }

            // check to see if thread was cancelled
            if (this.backgroundWorkerUpdateCheck.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (error == false)
            {
                lblShimmerCheck.ForeColor = Color.Black;
                picbxShimmerConnection.Image = ShimmerAPI.Properties.Resources.inprogress;

                if (!CheckForInternetConnection(Websites.Shimmer))
                {
                    picbxShimmerConnection.Image = ShimmerAPI.Properties.Resources.Tick_red;
                    string messageBoxText = "Could not connect with the Shimmer servers, please try again later or visit the Shimmer website.";
                    string caption = "Bootstrap Loader";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                    error = true;
                }
                else
                {
                    picbxShimmerConnection.Image = ShimmerAPI.Properties.Resources.Tick_green;
                }

                // check to see if thread was cancelled
                if (this.backgroundWorkerUpdateCheck.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (error == false)
                {
                    lblDownloading.ForeColor = Color.Black;
                    picbxDLUpdates.Image = ShimmerAPI.Properties.Resources.inprogress;


                    string[,] NewestVersions = GetNewestVersions();

                    //ChangeStatusLabel("Failed to check for updates");

                    if(NewestVersions == null)
                    {
                        picbxDLUpdates.Image = ShimmerAPI.Properties.Resources.Tick_red;
                        string messageBoxText = "Could not check for updates, please try again later or visit the Shimmer website.";
                        string caption = "Bootstrap Loader";
                        MessageBoxButtons button = MessageBoxButtons.OK;
                        MessageBoxIcon icon = MessageBoxIcon.Error;
                        MessageBox.Show(messageBoxText, caption, button, icon);
                        error = true;
                    }

                    // check to see if thread was cancelled
                    if (this.backgroundWorkerUpdateCheck.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (error == false)
                    {
                        string[,] CurrentVersions = GetCurrentVersions();
                        string[,] ListOfUpdates = CompareVersions(CurrentVersions, NewestVersions);

                        // Set the result to pass the array to completed event handler
                        e.Result = ListOfUpdates;
                    }
                }
            }
        }

        private void backgroundWorkerUpdateCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {
            // check to see if thread was cancelled
            if (e.Cancelled == true)
            {
                return;
            }

            if (e.Result != null)
            {
                // Get result objects 
                string[,] updatesList = (e.Result as string[,]);

                bool WebInstallRequired = false;
                bool ThisAppHasBeenDeprecated = false;
                int x = 0;
                for (x = 0; x < updatesList.GetLength(0); x++)
                {
                    if ((updatesList[x, (int)ListOfUpdatesColumnsOrder.major] == "X") && (updatesList[x, (int)ListOfUpdatesColumnsOrder.minor] == "X") && (updatesList[x, (int)ListOfUpdatesColumnsOrder.revision] == "X"))
                    {
                        ThisAppHasBeenDeprecated = true;
                        break;
                    }

                    if (updatesList[x, (int)ListOfUpdatesColumnsOrder.webinstall] == "1")
                    {
                        WebInstallRequired = true;
                        break;
                    }
                }

                if (ThisAppHasBeenDeprecated == true)
                {
                    if (updatesList[x, (int)ListOfUpdatesColumnsOrder.URL].Contains("www"))
                    {
                        MessageBox.Show("The " + updatesList[x, (int)ListOfUpdatesColumnsOrder.name] + " application has been discontinued. Please visit the Shimmer website for more information.");
                    }
                    else
                    {
                        MessageBox.Show("The " + updatesList[x, (int)ListOfUpdatesColumnsOrder.name] + " application has been discontinued and replaced by " + updatesList[x, (int)ListOfUpdatesColumnsOrder.URL] + ". Please visit the Shimmer website for more information.");
                    }
                }
                else if (WebInstallRequired == true)
                {
                    MessageBox.Show(updatesList[x, (int)ListOfUpdatesColumnsOrder.name] + " v" + updatesList[x, (int)ListOfUpdatesColumnsOrder.major] + "." + updatesList[x, (int)ListOfUpdatesColumnsOrder.minor] + "." + updatesList[x, (int)ListOfUpdatesColumnsOrder.revision] + " is available for download. Please visit the Shimmer website for more information.");
                }
                else if (updatesList.GetLength(0) > 0) // if downloadable updates exists, proceed to install form
                {
                    FormUpdateInstall InstallForm = new FormUpdateInstall(updatesList);
                    this.Hide();
                    InstallForm.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("Thank you for checking but there are no updates at this time.");
                }
            }

            if(File.Exists(Application.StartupPath + @"\InternalUse.txt"))
            {
                string messageBoxText = messageBoxText = "Check for internal releases?";
                string caption = "Bootstrap Loader";
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Information;
                DialogResult dialogResult = MessageBox.Show(this, messageBoxText, caption, button, icon);

                if (dialogResult == DialogResult.Yes)
                {
                    // check and install internal FW releases for BSL application
                    if (ThisAppUID == "1.3") // Bootstrap Loader
                    {
                        try
                        {
                            if (Directory.Exists(@"\\192.168.0.139\shimmer\Firmware"))
                            {
                                CheckForInternalReleases();
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            this.Close();
        }

        public static bool CheckForInternetConnection(Websites websites)
        {
            string link = string.Empty;
            if (websites == Websites.Google)
            {
                link = "http://www.google.com";
            }
            else if (websites == Websites.Baidu)
            {
                link = "http://www.baidu.com";
            }
            else if (websites == Websites.Shimmer)
            {
                link = "http://www.shimmersensing.com";
            }

            //// alternative with timeout
            //// http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.timeout(v=vs.110).aspx
            //// Create a new 'HttpWebRequest' Object to the mentioned URL.
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(link);
            //// Set the  'Timeout' property of the HttpWebRequest to 10 milliseconds.
            //myHttpWebRequest.Timeout = 10000;
            //try
            //{
            //    // A HttpWebResponse object is created and is GetResponse Property of the HttpWebRequest associated with it 
            //    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            //    if (myHttpWebResponse.StatusCode.Equals(HttpStatusCode.OK))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch
            //{
            //    return false;
            //}

            try
            {
                //TODO: Double check operate and tidy
                //TODO: Timeout
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(link))
                    {
                        if (stream != null)
                        {
                            stream.Dispose();
                            return true;
                        }
                        else
                        {
                            stream.Dispose();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public string[,] GetCurrentVersions()
        {
            // set up array
            string[,] CurrentVersions = new string[UpdatesToCheck.GetLength(0), (int)CurrentVersionsColumnsOrder.ArraySize];

            for (int i = 0; i < UpdatesToCheck.GetLength(0); i++)
            {
                // Store UID
                CurrentVersions[i, (int)CurrentVersionsColumnsOrder.UID] = UpdatesToCheck[i, 0];

                // find file path
                string CurrentFolder = string.Empty;
                if (UpdatesToCheck[i, 0] == ThisAppUID) // This application
                {
                    CurrentFolder = Application.StartupPath;
                    // Special case as no version number in the file name for this application
                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.UID] = CurrentVersions[i, (int)CurrentVersionsColumnsOrder.UID] + "." + versionNumber;
                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.major] = versionNumber.Split(new string[] { "." }, StringSplitOptions.None)[0];
                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.minor] = versionNumber.Split(new string[] { "." }, StringSplitOptions.None)[1];
                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.revision] = versionNumber.Split(new string[] { "." }, StringSplitOptions.None)[2];
                }
                else
                {
                if ((UpdatesToCheck[i, 0].Substring(0, UpdatesToCheck[i, 0].IndexOf(".", 0))) == "0") // Firmware
                {
                    CurrentFolder = Application.StartupPath + @"\" + FolderContainingFirmware;
                }
                else if ((UpdatesToCheck[i, 0].Substring(0, UpdatesToCheck[i, 0].IndexOf(".", 0))) == "1") // Software
                {
                    CurrentFolder = Application.StartupPath + @"\" + FolderContainingSoftware;
                }
                else if ((UpdatesToCheck[i, 0].Substring(0, UpdatesToCheck[i, 0].IndexOf(".", 0))) == "2") // Documentation
                {
                    CurrentFolder = Application.StartupPath + @"\" + FolderContainingDocumentation;
                }
                else
                {
                    CurrentFolder = Application.StartupPath;
                }

                // Find file info
                string[] files = Directory.GetFiles(CurrentFolder);
                foreach (string file in files)
                {
                    string FileNameNoExtension = Path.GetFileNameWithoutExtension(file);
                    if (FileNameNoExtension.Contains(UpdatesToCheck[i, 1]))
                    {
                        // Store file path
                        CurrentVersions[i, (int)CurrentVersionsColumnsOrder.path] = file;

                        if (FileNameNoExtension.Contains("v"))
                        {
                            string buffer = string.Empty;
                            buffer = FileNameNoExtension.Substring(FileNameNoExtension.LastIndexOf("v") + 1, FileNameNoExtension.Length - FileNameNoExtension.LastIndexOf("v") - 1);
                            // If version code contains a "letter" -> replace with ("." + "letter"). Find last instance of letter.
                            for (int x = buffer.Length - 1; x >= 0; x--)
                            {
                                if (char.IsLetter(buffer[x]))
                                {
                                    // Special case for Shimmer User Manual - will not work if file name changed from "*Shimmer User Manual*"
                                    if (FileNameNoExtension.Contains("Shimmer_User_Manual"))
                                    {
                                        buffer = buffer.Insert(x, ".0.");
                                        // Need to add more if version changes to for example v3RMe
                                    }
                                    else // Normal Conditions
                                    {
                                        buffer = buffer.Insert(x, ".");
                                    }
                                    break;
                                }
                            }
                            // If no "." -> pad
                            if (buffer.Count(x => x == '.') == 0)
                            {
                                buffer = buffer + ".0.0";
                            }
                            // If only one "." -> pad
                            if (buffer.Count(x => x == '.') == 1)
                            {
                                buffer = buffer + ".0";
                            }
                            //Correct number of '.' so save
                            if (buffer.Count(x => x == '.') == 2)
                            {
                                CurrentVersions[i, (int)CurrentVersionsColumnsOrder.UID] = CurrentVersions[i, (int)CurrentVersionsColumnsOrder.UID] + "." + buffer;
                                string[] RevisionBuffer = buffer.Split(new string[] { "." }, StringSplitOptions.None);
                                if (RevisionBuffer.Length == 3)
                                {
                                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.major] = RevisionBuffer[0];
                                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.minor] = RevisionBuffer[1];
                                    CurrentVersions[i, (int)CurrentVersionsColumnsOrder.revision] = RevisionBuffer[2];
                                }
                            }
                        }
                    }
                        break;
                    }
                }
            }
            return CurrentVersions;
        }


        public string[,] GetNewestVersions()
        {
            //ChangeStatusLabel("Checking for updates");

            // set up array
            string[,] NewestVersions = new string[UpdatesToCheck.GetLength(0), (int)NewsestVersionsColumnsOrder.ArraySize];

            //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebRequest objHttpWebRequest;
            //Declare an implementation of the WebResponse class
            WebResponse objWebResponse = null;
            //Declare XMLReader
            XmlTextReader objXMLReader = null;
            //Creates an HttpWebRequest for the specified URL.
            objHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.shimmersensing.com/5368696d6d6572");

            try
            {
                objWebResponse = objHttpWebRequest.GetResponse();

                objXMLReader = new XmlTextReader(objWebResponse.ResponseUri.AbsoluteUri.ToString());

                this.picbxDLUpdates.Image = ShimmerAPI.Properties.Resources.Tick_green;
                objXMLReader.MoveToContent();
                string elementName = "";
                if ((objXMLReader.NodeType == XmlNodeType.Element) && (objXMLReader.Name == "shimmerUpdates"))
                {
                    bool EntryFound = false;
                    int ArrayIndex = 0;
                    while (objXMLReader.Read())
                    {
                        if (objXMLReader.NodeType == XmlNodeType.Element)
                        {
                            elementName = objXMLReader.Name;
                        }
                        else
                        {
                            if ((objXMLReader.NodeType == XmlNodeType.Text) && (objXMLReader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "UID":
                                        EntryFound = false;
                                        // Split UID
                                        int count = 0;
                                        int x = 0;
                                        for (x = 0; x < objXMLReader.Value.Length; x++)
                                        {
                                            if (objXMLReader.Value[x] == '.')
                                            {
                                                count++;
                                                if (count == 2)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        string UI = objXMLReader.Value.Substring(0, x);

                                        for (int y = 0; y < UpdatesToCheck.GetLength(0) ; y++ )
                                        {
                                            if(UI == UpdatesToCheck[y, 0])
                                            {
                                                ArrayIndex = y;
                                                NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.UID] = objXMLReader.Value;
                                                EntryFound = true;
                                                break;
                                            }
                                        }
                                        break;
                                    case "device":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.device] = objXMLReader.Value;
                                        }
                                        break;
                                    case "category":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.category] = objXMLReader.Value;
                                        }
                                        break;
                                    case "subcategory":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.subcategory] = objXMLReader.Value;
                                        }
                                        break;
                                    case "name":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.name] = objXMLReader.Value;
                                        }
                                        break;
                                    case "major":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.major] = objXMLReader.Value;
                                        }
                                        break;
                                    case "minor":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.minor] = objXMLReader.Value;
                                        }
                                        break;
                                    case "revision":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.revision] = objXMLReader.Value;
                                        }
                                        break;
                                    case "day":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.day] = objXMLReader.Value;
                                        }
                                        break;
                                    case "month":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.month] = objXMLReader.Value;
                                        }
                                        break;
                                    case "year":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.year] = objXMLReader.Value;
                                        }
                                        break;
                                    case "URL":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.URL] = objXMLReader.Value;
                                        }
                                        break;
                                    case "checksum":
                                        if (EntryFound)
                                        {
                                            NewestVersions[ArrayIndex, (int)NewsestVersionsColumnsOrder.checksum] = objXMLReader.Value;
                                        }
                                        break;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                NewestVersions = null;
            }
            finally
            {
                //Close connections
                if (objWebResponse != null)
                    objWebResponse.Close();

                //Release objects
                objXMLReader = null;
                objWebResponse = null;
                objHttpWebRequest = null;

                if (objXMLReader != null)
                    objXMLReader.Close();

            }

            return NewestVersions;
        }

        public string[,] CompareVersions(string[,] currentVersions, string[,] newestVersions)
        {
            bool[] versionsToUpdate = new bool[currentVersions.GetLength(0)];
            int[] versionsToUpdateBuffer = new int[currentVersions.GetLength(0)];
            int UpdatesCounter = 0;

            for (int i = 0; i < currentVersions.Length / (int)CurrentVersionsColumnsOrder.ArraySize; i++)
            {
                versionsToUpdate[i] = false;
                versionsToUpdateBuffer[i] = 0;

                if ((newestVersions[i, (int)NewsestVersionsColumnsOrder.UID] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.device] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.name] == null)
                    || (newestVersions[i, (int)NewsestVersionsColumnsOrder.major] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.minor] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.revision] == null)
                    || (newestVersions[i, (int)NewsestVersionsColumnsOrder.day] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.month] == null) || (newestVersions[i, (int)NewsestVersionsColumnsOrder.year] == null) 
                    || (newestVersions[i, (int)NewsestVersionsColumnsOrder.URL] == null) )
                {
                    // If any of the essential XML elements contain nulls, return false 
                    versionsToUpdateBuffer[i] = 0;
                }
                else if (currentVersions[i, (int)CurrentVersionsColumnsOrder.major] == null) 
                {
                    // If no file present on computer, update
                    versionsToUpdateBuffer[i] = 1;
                }
                else if ((currentVersions[i, (int)CurrentVersionsColumnsOrder.major] == "X") && (currentVersions[i, (int)CurrentVersionsColumnsOrder.minor] == "X") && (currentVersions[i, (int)CurrentVersionsColumnsOrder.revision] == "X"))
                {
                    // If current item has been depricated, signal to update - handle consequence else where
                    versionsToUpdateBuffer[i] = 1;
                }
                else
                {
                    // Test major charactor
                    versionsToUpdateBuffer[i] = newestVersions[i, (int)NewsestVersionsColumnsOrder.major].CompareTo(currentVersions[i, (int)CurrentVersionsColumnsOrder.major]); //< 0 
                    
                    if (versionsToUpdateBuffer[i] != 1)
                    {
                        // Test minor version
                        versionsToUpdateBuffer[i] = newestVersions[i, (int)NewsestVersionsColumnsOrder.minor].CompareTo(currentVersions[i, (int)CurrentVersionsColumnsOrder.minor]); //< 0 
                        if (versionsToUpdateBuffer[i] != 1)
                        {
                            // Test revision
                            versionsToUpdateBuffer[i] = newestVersions[i, (int)NewsestVersionsColumnsOrder.revision].CompareTo(currentVersions[i, (int)CurrentVersionsColumnsOrder.revision]); //< 0 
                        }
                    }
                }

                if (versionsToUpdateBuffer[i] > 0)
                {
                    versionsToUpdate[i] = true;
                    UpdatesCounter += 1;
                }
                else
                {
                    versionsToUpdate[i] = false;
                }
            }

            string[,] listOfUpdates = new string[UpdatesCounter, (int)ListOfUpdatesColumnsOrder.ArraySize];
            int listOfUpdatesIndex = 0;

            for(int x = 0; x < versionsToUpdate.GetLength(0); x++)
            {
                if (versionsToUpdate[x] == true)
                {
                    for (int y = 0; y < newestVersions.GetLength(1); y++)
                    {
                        listOfUpdates[listOfUpdatesIndex, y] = newestVersions[x, y];
                    }
                    // add old file path to results array
                    listOfUpdates[listOfUpdatesIndex, (int)ListOfUpdatesColumnsOrder.path] = currentVersions[x, (int)CurrentVersionsColumnsOrder.path];
                    // add indicator as to whether the user must proceed to Shimmer Website to proceed
                    listOfUpdates[listOfUpdatesIndex, (int)ListOfUpdatesColumnsOrder.webinstall] = this.UpdatesToCheck[x, 2];
                    listOfUpdatesIndex += 1;
                }
            }

            return listOfUpdates;
        }


        // Function only for internal Shimmer use
        private void CheckForInternalReleases()
        {
            string currentFolder = string.Empty;
            string currentFile = string.Empty;

            string feedbackMessage = "The following internal firmware releases were updated:\r";
            bool newInternalRelease = false;

            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    currentFolder = @"\\192.168.0.139\shimmer\Firmware\Shimmer3\BtStream";
                    currentFile = "BtStream_Shimmer3*.txt";
                }
                else if (i == 1)
                {
                    currentFolder = @"\\192.168.0.139\shimmer\Firmware\Shimmer3\LogAndStream";
                    currentFile = "LogAndStream_Shimmer3*.txt";
                }
                else if (i == 2)
                {
                    currentFolder = @"\\192.168.0.139\shimmer\Firmware\Shimmer3\SDLog";
                    currentFile = "SDLog_Shimmer3*.txt";
                }

                if (Directory.Exists(currentFolder))
                {
                    //                        string[] files = Directory.GetFiles(currentFolder, currentFile);

                    var files = new DirectoryInfo(currentFolder).GetFiles(currentFile)
                    .OrderByDescending(f => f.CreationTime)
                    .Select(f => f.FullName)
                    .ToList();

                    if (files.Count() > 0)
                    {
                        // check if not external release and if file doesn't already exist in user bootstraps directory
                        if (!Path.GetFileName(files[0]).Contains(".0.txt") && !File.Exists(Application.StartupPath + @"\Bootstraps\User Bootstraps\" + Path.GetFileName(files[0])))
                        {
                            try
                            {
                                File.Copy(files[0], Application.StartupPath + @"\Bootstraps\User Bootstraps\" + Path.GetFileName(files[0]));
                                feedbackMessage += "      " + Path.GetFileName(files[0]) + "\r";
                                newInternalRelease = true;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            if (newInternalRelease.Equals(false))
            {
                feedbackMessage = "No new internal firmware releases at this time.";
            }

            string messageBoxText = feedbackMessage;
            string caption = "Bootstrap Loader";
            MessageBoxButtons button = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.None;
            MessageBox.Show(this, messageBoxText, caption, button, icon);
        }


    }
}