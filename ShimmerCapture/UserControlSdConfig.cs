using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ShimmerAPI
{
    public partial class UserControlSdConfig : UserControl
    {
        public bool ChangeSuccesful  = false;
        public Configuration PConfiguration;
        public static bool usingLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
        public UserControlSdConfig()
        {
            InitializeComponent();
        }
        //===================================================================
        //===================================================================
        //===================================================================
        //==================== btsd changes from Control.cs =================
        //===================================================================
        //===================================================================
        //===================================================================
        // btsd changes
        public bool isLogging;
        public bool isConnected;
        public string status_text;



        private void btnApply_Click(object sender, EventArgs e)
        {
            /*btnOK.Text = "Applying";
            btnOK.Enabled = false;
            ApplySDConfigurations();
            btnOK.Text = "Apply";
            btnOK.Enabled = true;*/
        }

        public void ApplySDConfigurations()
        {
            if (PConfiguration == null)
            {
                PConfiguration = (Configuration)this.Parent.Parent.Parent;
            }

            bool param_changed = false;
            bool trial_changed = false;
            //PConfiguration.PControlForm.shimmer
            if (PConfiguration.PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_STREAMING)
            {
                MessageBox.Show("Cannot change configure settings while streaming data. ", Control.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (PConfiguration.PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_CONNECTING)
            {
                MessageBox.Show("Cannot change configure settings while building configuration ", Control.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (PConfiguration.PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_NONE)
            {
                MessageBox.Show("Please connect to a shimmer before configuration.", Control.ApplicationName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (chBxIAmMaster.Checked != PConfiguration.PControlForm.ShimmerDevice.GetIAmMaster())
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetIAmMaster(chBxIAmMaster.Checked);
                    //PConfiguration.PControlForm.shimmer.changeTrial = true;
                    trial_changed = true;
                }
                if (txtBxCenter.Text != PConfiguration.PControlForm.ShimmerDevice.GetCenter())
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetCenter(txtBxCenter.Text);
                    param_changed = true;
                    PConfiguration.PControlForm.ShimmerDevice.WriteCenter();
                }
                if (!string.IsNullOrEmpty(txtBxShimmerName.Text) && txtBxShimmerName.Text != PConfiguration.PControlForm.ShimmerDevice.GetShimmerName())
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetShimmerName(txtBxShimmerName.Text);
                    param_changed = true;
                    PConfiguration.PControlForm.ShimmerDevice.WriteShimmerName();
                }

                if (!string.IsNullOrEmpty(txtBxExpID.Text) && txtBxExpID.Text != PConfiguration.PControlForm.ShimmerDevice.GetExperimentID())
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetExperimentID(txtBxExpID.Text);
                    param_changed = true;
                    PConfiguration.PControlForm.ShimmerDevice.WriteExpID();
                }
                if (txtBxMyID.Text.Length != 0)
                {
                    if (Convert.ToInt32(txtBxMyID.Text) != PConfiguration.PControlForm.ShimmerDevice.GetMyID())
                    {
                        PConfiguration.PControlForm.ShimmerDevice.SetMyID(Convert.ToInt32(txtBxMyID.Text));
                        param_changed = true;
                        PConfiguration.PControlForm.ShimmerDevice.WriteMyID();
                    }
                }
                if (txtBxNshimmer.Text.Length != 0)
                {

                    if (Convert.ToInt32(txtBxNshimmer.Text) != PConfiguration.PControlForm.ShimmerDevice.GetNshimmer())
                    {
                        PConfiguration.PControlForm.ShimmerDevice.SetNshimmer(Convert.ToInt32(txtBxNshimmer.Text));
                        param_changed = true;
                        PConfiguration.PControlForm.ShimmerDevice.WriteNshimmer();
                    }
                }

                if (chBxSync.Checked != PConfiguration.PControlForm.ShimmerDevice.GetSync())
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetSync(chBxSync.Checked);
                    trial_changed = true;
                }
                if (PConfiguration.PControlForm.ShimmerDevice.GetInterval() != Convert.ToInt32(numUpDownInterval.Value))
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetInterval(Convert.ToInt32(numUpDownInterval.Value));
                    trial_changed = true;
                }

                //mode logic
                if ((radBtnSingletouch.Checked != PConfiguration.PControlForm.ShimmerDevice.GetSingleTouch()) ||
                    (radBtnUserButton.Checked != PConfiguration.PControlForm.ShimmerDevice.GetUserButton()))
                {
                    if (radBtnSingletouch.Checked)
                    {
                        if (!PConfiguration.PControlForm.ShimmerDevice.GetSingleTouch() || !PConfiguration.PControlForm.ShimmerDevice.GetUserButton())
                            trial_changed = true;
                        PConfiguration.PControlForm.ShimmerDevice.SetSingleTouch(true);
                        PConfiguration.PControlForm.ShimmerDevice.SetUserButton(true);
                    }
                    else if (radBtnUserButton.Checked)
                    {
                        if (PConfiguration.PControlForm.ShimmerDevice.GetSingleTouch() || !PConfiguration.PControlForm.ShimmerDevice.GetUserButton())
                            trial_changed = true;
                        PConfiguration.PControlForm.ShimmerDevice.SetSingleTouch(false);
                        PConfiguration.PControlForm.ShimmerDevice.SetUserButton(true);
                    }
                    else if (radBtnAutoRun.Checked)
                    {
                        if (PConfiguration.PControlForm.ShimmerDevice.GetSingleTouch() || PConfiguration.PControlForm.ShimmerDevice.GetUserButton())
                            trial_changed = true;
                        PConfiguration.PControlForm.ShimmerDevice.SetSingleTouch(false);
                        PConfiguration.PControlForm.ShimmerDevice.SetUserButton(false);
                    }
                }
                if (trial_changed)
                {
                    PConfiguration.PControlForm.ShimmerDevice.WriteTrial();
                }


                if (trial_changed || param_changed)
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetConfigTime(Convert.ToInt32(PConfiguration.PControlForm.ShimmerDevice.SystemTime2Config()));
                    PConfiguration.PControlForm.ShimmerDevice.WriteConfigTime();
                    txtBxConfigTime.Text = PConfiguration.PControlForm.ShimmerDevice.ConfigTimeToShowString(PConfiguration.PControlForm.ShimmerDevice.GetConfigTime());
                }

                //PConfiguration.PControlForm.ShimmerDevice.WriteSdConfigFile();

                if (trial_changed || param_changed)
                {
                    ChangeSuccesful = true;
                    
                }
            }

        }

        private void radBtnUserButton_CheckedChanged(object sender, EventArgs e)
        {

            enable_configure_OK();
        }

        private void UserControlSdConfig_Load(object sender, EventArgs e)
        {
            if (usingLinux)
            {
                this.Width = 880;   //+85
                this.Height = 459;  //+20
            }
            
            try
            {
                PConfiguration = (Configuration)this.Parent.Parent.Parent;
                System.Console.WriteLine(PConfiguration.Name);

                SD_params();//(sender, e);

                // btsd changes

                chBxIAmMaster.Checked = PConfiguration.PControlForm.ShimmerDevice.GetIAmMaster();
                txtBxCenter.Text = PConfiguration.PControlForm.ShimmerDevice.GetCenter();
                txtBxShimmerName.Text = PConfiguration.PControlForm.ShimmerDevice.GetShimmerName();
                txtBxExpID.Text = PConfiguration.PControlForm.ShimmerDevice.GetExperimentID();
                txtBxMyID.Text = Convert.ToString(PConfiguration.PControlForm.ShimmerDevice.GetMyID());
                txtBxNshimmer.Text = Convert.ToString(PConfiguration.PControlForm.ShimmerDevice.GetNshimmer());
                txtBxConfigTime.Enabled = false;
                txtBxConfigTime.Text = PConfiguration.PControlForm.ShimmerDevice.ConfigTimeToShowString(PConfiguration.PControlForm.ShimmerDevice.GetConfigTime());
                int interval_time = PConfiguration.PControlForm.ShimmerDevice.GetInterval();
                if (interval_time >= 54 && interval_time <= 255)
                {
                    numUpDownInterval.Value = interval_time;
                }
                else
                {
                    PConfiguration.PControlForm.ShimmerDevice.SetInterval(54);
                    numUpDownInterval.Value = 54;
                }

                if (PConfiguration.PControlForm.ShimmerDevice.GetSingleTouch())
                {
                    radBtnSingletouch.Checked = true;
                    radBtnUserButton.Checked = false;
                    radBtnAutoRun.Checked = false;
                }
                else if (PConfiguration.PControlForm.ShimmerDevice.GetUserButton())
                {
                    radBtnSingletouch.Checked = false;
                    radBtnUserButton.Checked = true;
                    radBtnAutoRun.Checked = false;
                }
                else
                {
                    radBtnSingletouch.Checked = false;
                    radBtnUserButton.Checked = false;
                    radBtnAutoRun.Checked = true;
                }
                chBxSync.Checked = PConfiguration.PControlForm.ShimmerDevice.GetSync();

                buttonApplySd.Enabled = false;

            }
            catch (InvalidCastException )
            {

            }
            catch (NullReferenceException)
            {

            }

        }

        // btsd changes
        //private void chBxSD_CheckedChanged(object sender, EventArgs e)
        private void SD_params()//(object sender, EventArgs e)
        {
            chBxIAmMaster.Enabled = false;
            txtBxCenter.Enabled = false;
            txtBxShimmerName.Enabled = true;
            txtBxExpID.Enabled = true;
            txtBxMyID.Enabled = false;
            txtBxNshimmer.Enabled = false;
            chBxSync.Enabled = false;
            numUpDownInterval.Enabled = false;
            label12.Enabled = false;
            // todo : in later versions change here
            radBtnSingletouch.Enabled = false;
            radBtnUserButton.Enabled = true;
            radBtnAutoRun.Enabled = true;
            //below are pure texts
            label7.Enabled = false;
            label8.Enabled = true;
            label9.Enabled = true;
            label10.Enabled = false;
            label11.Enabled = false;
            chBxIAmMaster_CheckedChanged();//(sender, e);
            chBxSync_CheckedChanged();//(sender, e);


        }

        // btsd changes
        private void chBxIAmMaster_CheckedChanged()//(object sender, EventArgs e)
        {
            enable_configure_OK();
            if (chBxIAmMaster.Checked)
            {
                txtBxCenter.Enabled = false;
            }
            else
            {
                //txtBxCenter.Enabled = true;
            }

        }

        // btsd changes
        private void chBxSync_CheckedChanged()//(object sender, EventArgs e)
        {

            enable_configure_OK();
            if (chBxSync.Checked)
            {
                numUpDownInterval.Enabled = true;
            }
            else
            {
                if (radBtnSingletouch.Checked)
                {
                    MessageBox.Show("Sync must be set on in Single Touch mode", ShimmerSDBT.AppNameCapture,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chBxSync.Checked = true;
                }
                else
                    numUpDownInterval.Enabled = false;
            }
        }

        private void enable_configure_OK()
        {
            try
            {
                if (PConfiguration.PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_STREAMING)
                    buttonApplySd.Enabled = false;
                else
                    buttonApplySd.Enabled = true;
            }
            catch { }
        }

        private void chBxIAmMaster_CheckedChanged_1(object sender, EventArgs e)         {            enable_configure_OK();        }
        private void txtBxCenter_TextChanged(object sender, EventArgs e)                {            enable_configure_OK();        }
        private void txtBxShimmerName_TextChanged(object sender, EventArgs e)           {            enable_configure_OK();        }
        private void txtBxExpID_TextChanged(object sender, EventArgs e)                 {            enable_configure_OK();        }
        private void txtBxMyID_TextChanged(object sender, EventArgs e)                  {            enable_configure_OK();        }
        private void txtBxNshimmer_TextChanged(object sender, EventArgs e)              {            enable_configure_OK();        }
        private void radBtnSingletouch_CheckedChanged(object sender, EventArgs e)       {            enable_configure_OK();        }
        private void radBtnAutoRun_CheckedChanged(object sender, EventArgs e)           {            enable_configure_OK();        }
        private void chBxSync_CheckedChanged_1(object sender, EventArgs e)              {            enable_configure_OK();        }
        private void numUpDownInterval_ValueChanged(object sender, EventArgs e)         {            enable_configure_OK();        }

        private void buttonApplySd_Click(object sender, EventArgs e)
        {
            buttonApplySd.Text = "Applying";
            buttonApplySd.Enabled = false;
            ApplySDConfigurations();
            buttonApplySd.Text = "Apply";
            buttonApplySd.Enabled = false;
            if (ChangeSuccesful)
            {
                PConfiguration.PControlForm.AppendTextBox("Configuration done.");
                MessageBox.Show("Configurations changed.", ShimmerSDBT.AppNameCapture,
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeSuccesful = false;
                
            } 
        }
    }
}
