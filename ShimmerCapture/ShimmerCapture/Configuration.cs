using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShimmerAPI
{
    public partial class Configuration : Form
    {
        public byte[] ExgReg1UI= {0,0,0,0,0,0,0,0,0,0};
        public byte[] ExgReg2UI = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int EnabledSensorsUI = 0;
        public Control PControlForm;
        protected internal Boolean ExgTabOpened = false;
        
        public Configuration()
        {
            InitializeComponent();
        }

        public Configuration(Control controlForm)
            : this()
        {
            PControlForm = controlForm;
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            Color backColor = this.BackColor;
            this.tabPage1.BackColor = backColor;
            this.tabPage2.BackColor = backColor;
            //this.tabPage3.BackColor = backColor;
            tabControl1.TabPages[2].Text = "Logging Options";
            tabControl1.TabPages[1].Text = "Advanced ExG";

            if (PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_STREAMING
                || PControlForm.ShimmerDevice.GetState() == Shimmer.SHIMMER_STATE_NONE)
            {
                tabControl1.TabPages[0].Enabled = false;
                tabControl1.TabPages[1].Enabled = false;
                tabControl1.TabPages[2].Enabled = false;
                buttonOk.Enabled = false;
            }
            else
            {
                //Only enable ExG config if Shimmer3 and BtStream > 0.2.8 and if ExG sensors are enabled
                int enabledSensors = PControlForm.ShimmerDevice.GetEnabledSensors();

                if (PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    ExgReg1UI = PControlForm.ShimmerDevice.GetEXG1RegisterContents();
                    ExgReg2UI = PControlForm.ShimmerDevice.GetEXG2RegisterContents();
                    EnabledSensorsUI = PControlForm.ShimmerDevice.GetEnabledSensors();
                }

                if ((((PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                && ((PControlForm.ShimmerDevice.GetFirmwareVersion() == 0.2 & PControlForm.ShimmerDevice.GetFirmwareInternal() >= 8)
                || (PControlForm.ShimmerDevice.GetFirmwareVersion() >= 0.3)))
                || PControlForm.ShimmerDevice.GetFirmwareIdentifier() == 3)
                && (((enabledSensors & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((enabledSensors & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0)
                || ((enabledSensors & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0) || ((enabledSensors & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)))
                {
                    tabControl1.TabPages[1].Enabled = true;
                    
                }
                else
                {
                    tabControl1.TabPages[1].Enabled = false;
                }

                if (PControlForm.ShimmerDevice.GetFirmwareIdentifier() == 3)
                {
                    tabControl1.TabPages[2].Enabled = true;
                }
                else
                {
                    tabControl1.TabPages[2].Enabled = false;
                    
                }
                buttonOk.Enabled = true;
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            int ind = tabControl1.SelectedIndex;
            if (ind == 0)
            {
                if (userControlExgConfig1.TestTextBoxExGConfigurations()) // check first if they are any invalid ExGConfigurations
                {
                    ExgReg1UI = userControlExgConfig1.getByteEXG1FromForm();
                    ExgReg2UI = userControlExgConfig1.getByteEXG2FromForm();
                }
                else
                {
                    /*
                    MessageBox.Show("Invalid value found in EXG Chip register text box, reverting to old values", Control.ApplicationName,
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    */
                }
                userControlGeneralConfig1.ForceExGConfigurationUpdate(ExgReg1UI,ExgReg2UI);
                
            }
            else if (ind == 1)
            {
                EnabledSensorsUI = userControlGeneralConfig1.GetUIEnabledSensors();
                userControlExgConfig1.ForceExGConfigurationUpdate();
                ExgTabOpened = true;
            }
            else if (ind == 2)  //SD Log
            {

            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            buttonOk.Text = "Configuring";
            buttonOk.Enabled = false;
            buttonCancel.Enabled = false;
            if (userControlGeneralConfig1.comboBoxBaudRate.SelectedIndex != PControlForm.ShimmerDevice.GetBaudRate() && PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                userControlGeneralConfig1.BaudRateChangeFlag = true;
            }
            else
            {
                userControlGeneralConfig1.BaudRateChangeFlag = false;
            }
            userControlGeneralConfig1.ApplyConfigurationChanges();
            if (!userControlGeneralConfig1.BaudRateChangeFlag)
            {
                updateConfigurations();
            }
        }

        private void updateConfigurations()
        {
            if (ExgTabOpened)
            {
                if (PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
                {
                    userControlExgConfig1.setExGRegBytesinForm();
                }
                userControlExgConfig1.ApplyExgConfigurations();
            }

            if (PControlForm.ShimmerDevice.GetFirmwareIdentifier() == 3)
            {
                PControlForm.ShimmerDevice.SetConfigTime(Convert.ToInt32(PControlForm.ShimmerDevice.SystemTime2Config()));
                PControlForm.ShimmerDevice.WriteConfigTime();
                userControlSdConfig1.ApplySDConfigurations();
            }

            buttonOk.Text = "OK";
            buttonOk.Enabled = true;
            buttonCancel.Enabled = true;
            userControlGeneralConfig1.BaudRateChangeFlag = false;
            PControlForm.ShimmerDevice.ReadShimmerName();
            PControlForm.ShimmerDevice.ReadExpID();
            PControlForm.ShimmerDevice.ReadConfigTime();
            MessageBox.Show("Configurations changed.", ShimmerSDBT.AppNameCapture,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void userControlGeneralConfig1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}
