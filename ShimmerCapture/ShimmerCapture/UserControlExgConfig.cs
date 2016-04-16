using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShimmerAPI
{
    public partial class UserControlExgConfig : UserControl
    {
        Configuration PConfiguration;
        byte[] defaultECGReg1 = new byte[10] {0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03};
        byte[] defaultECGReg2 = new byte[10] {0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01};
        byte[] defaultEMGReg1 = new byte[10] {0x00, 0xA0, 0x10, 0x69, 0x60, 0x20, 0x00, 0x00, 0x02, 0x03};
        byte[] defaultEMGReg2 = new byte[10] {0x00, 0xA0, 0x10, 0xE1, 0xE1, 0x00, 0x00, 0x00, 0x02, 0x01};
        byte[] defaultExGTestReg1 = new byte[10] {0x00, 0xA3, 0x10, 0x45, 0x45, 0x00, 0x00, 0x00, 0x02, 0x01};
        byte[] defaultExGTestReg2 = new byte[10] { 0x00, 0xA3, 0x10, 0x45, 0x45, 0x00, 0x00, 0x00, 0x02, 0x01 };

        public static bool UsingLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
        public UserControlExgConfig()
        {
            InitializeComponent();
        }

        private void UserControlExgConfig_Load(object sender, EventArgs e)
        {
            if (UsingLinux)
            {
                this.Width = 880;   //+85
                this.Height = 459;  //+20
            }
            try
            {
                

                PConfiguration = (Configuration)this.Parent.Parent.Parent;

                comboBoxExGReferenceElectrode.Items.AddRange(Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES);
                comboBoxExGReferenceElectrode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxExGReferenceElectrode.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxExGReferenceElectrode.DropDownStyle = ComboBoxStyle.DropDownList;

                comboBoxLeadOffDetection.Items.AddRange(Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS);
                comboBoxLeadOffDetection.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxLeadOffDetection.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxLeadOffDetection.DropDownStyle = ComboBoxStyle.DropDownList;

                comboBoxExGLeadOffCurrent.Items.AddRange(Shimmer.LIST_OF_EXG_LEAD_OFF_CURRENTS);
                comboBoxExGLeadOffCurrent.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxExGLeadOffCurrent.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxExGLeadOffCurrent.DropDownStyle = ComboBoxStyle.DropDownList;

                comboBoxLeadOffComparatorThreshold.Items.AddRange(Shimmer.LIST_OF_EXG_LEAD_OFF_COMPARATOR_THRESHOLDS);
                comboBoxLeadOffComparatorThreshold.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxLeadOffComparatorThreshold.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBoxLeadOffComparatorThreshold.DropDownStyle = ComboBoxStyle.DropDownList;

                checkBoxHPF0_05.Enabled = true;
                checkBoxHPF0_5.Enabled = true;
                checkBoxHPF5.Enabled = true;
                checkBoxBSF50.Enabled = true;
                checkBoxBSF60.Enabled = true;
                checkBoxNQFilter.Enabled = true;

                LoadExGConfigurationFromMainForm();

                checkBoxHPF0_05.Checked = false;
                checkBoxHPF0_5.Checked = false;
                checkBoxHPF5.Checked = false;
                checkBoxBSF50.Checked = false;
                checkBoxBSF60.Checked = false;
                checkBoxNQFilter.Checked = false;

                if (PConfiguration.PControlForm.EnableNQF == true)
                {
                    checkBoxNQFilter.Checked = true;
                }

                if (PConfiguration.PControlForm.EnableHPF_0_05HZ == true)
                {
                    checkBoxHPF0_05.Checked = true;
                    checkBoxHPF0_5.Checked = false;
                    checkBoxHPF5.Checked = false;
                }
                else if (PConfiguration.PControlForm.EnableHPF_0_5HZ == true)
                {
                    checkBoxHPF0_05.Checked = false;
                    checkBoxHPF0_5.Checked = true;
                    checkBoxHPF5.Checked = false;
                }
                else if (PConfiguration.PControlForm.EnableHPF_5HZ == true)
                {
                    checkBoxHPF0_05.Checked = false;
                    checkBoxHPF0_5.Checked = false;
                    checkBoxHPF5.Checked = true;
                }

                if (PConfiguration.PControlForm.EnableBSF_49_51HZ == true)
                {
                    checkBoxBSF50.Checked = true;
                    checkBoxBSF60.Checked = false;
                }
                else if (PConfiguration.PControlForm.EnableBSF_59_61HZ == true)
                {
                    checkBoxBSF50.Checked = false;
                    checkBoxBSF60.Checked = true;
                }
            }
            catch (InvalidCastException)
            {

            }
            catch (NullReferenceException)
            {

            }
        }

        private void checkBoxHPF0_05_Click(object sender, EventArgs e)
        {
            if (checkBoxHPF0_05.Checked)
            {
                checkBoxHPF0_5.Checked = false;
                checkBoxHPF5.Checked = false;
            }
        }

        private void checkBoxHPF0_5_Click(object sender, EventArgs e)
        {
            if (checkBoxHPF0_5.Checked)
            {
                checkBoxHPF0_05.Checked = false;
                checkBoxHPF5.Checked = false;
            }
        }

        private void checkBoxHPF5_Click(object sender, EventArgs e)
        {
            if (checkBoxHPF5.Checked)
            {
                checkBoxHPF0_05.Checked = false;
                checkBoxHPF0_5.Checked = false;
            }
        }

        private void checkBoxBSF50_Click(object sender, EventArgs e)
        {
            if (checkBoxBSF50.Checked)
            {
                if (PConfiguration.PControlForm.ShimmerDevice.GetSamplingRate() < 100)
                {
                    MessageBox.Show("Warning Shimmer sampling rate to low, should be at least 100Hz", Control.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    checkBoxBSF50.Checked = false;
                }
                else
                {
                    checkBoxBSF60.Checked = false;
                }
            }
        }

        private void checkBoxBSF60_Click(object sender, EventArgs e)
        {
            if (checkBoxBSF60.Checked)
            {
                if (PConfiguration.PControlForm.ShimmerDevice.GetSamplingRate() < 120)
                {
                    MessageBox.Show("Warning Shimmer sampling rate to low, should be at least 120Hz", Control.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    checkBoxBSF60.Checked = false;
                }
                else
                {
                    checkBoxBSF50.Checked = false;
                }
            }
        }

        private void checkBoxDefaultECG_Click(object sender, EventArgs e)
        {
            if (checkBoxDefaultECG.Checked)
            {
                Array.Copy(defaultECGReg1, PConfiguration.ExgReg1UI, 10);
                Array.Copy(defaultECGReg2, PConfiguration.ExgReg2UI, 10);
                PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedIndex = 3; // set recommended gain index for ECG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked = true; // enable ECG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked = false; // disable EMG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor17.Checked = false; // disable ExG Test
                checkBoxDefaultEMG.Checked = false;
                checkBoxDefaultExGTest.Checked = false;
                setExGUIElements();
                setExGRegBytesinForm();
            }
            else
            {
                
            }
        }

        private void checkBoxDefaultEMG_Click(object sender, EventArgs e)
        {
            if (checkBoxDefaultEMG.Checked)
            {
                Array.Copy(defaultEMGReg1, PConfiguration.ExgReg1UI, 10);
                Array.Copy(defaultEMGReg2, PConfiguration.ExgReg2UI, 10);
                PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedIndex = 6; // set recommended gain index for EMG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked = false; // disable ECG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked = true; // enable EMG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor17.Checked = false; // disable ExG Test
                checkBoxDefaultECG.Checked = false;
                checkBoxDefaultExGTest.Checked = false;
                setExGUIElements();
                setExGRegBytesinForm();
            }
            else
            {

            }

        }

        private void checkBoxDefaultExGTest_Click(object sender, EventArgs e)
        {
            if (checkBoxDefaultExGTest.Checked)
            {
                Array.Copy(defaultExGTestReg1, PConfiguration.ExgReg1UI, 10);
                Array.Copy(defaultExGTestReg2, PConfiguration.ExgReg2UI, 10);
                PConfiguration.ExgReg1UI = defaultExGTestReg1;
                PConfiguration.ExgReg2UI = defaultExGTestReg2;
                PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedIndex = 3; // set recommended gain index for ExG Test
                PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked = false; // disable ECG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked = false; // disable EMG
                PConfiguration.userControlGeneralConfig1.checkBoxSensor17.Checked = true; // enable ExG Test
                checkBoxDefaultECG.Checked = false;
                checkBoxDefaultEMG.Checked = false;
                setExGUIElements();
                setExGRegBytesinForm();
            }
            else
            {

            }

        }
        
        private Boolean isDefaultECG() // compare ExG Register bytes in form with the default for ECG
        {
            var arraysAreEqual = Enumerable.SequenceEqual(PConfiguration.ExgReg1UI, defaultECGReg1) && Enumerable.SequenceEqual(PConfiguration.ExgReg2UI, defaultECGReg2);
            return arraysAreEqual;
        }

        private Boolean isDefaultEMG() // compare ExG Register bytes in form with the default for EMG
        {
            var arraysAreEqual = Enumerable.SequenceEqual(PConfiguration.ExgReg1UI, defaultEMGReg1) && Enumerable.SequenceEqual(PConfiguration.ExgReg2UI, defaultEMGReg2);
            return arraysAreEqual;
        }

        private Boolean isDefaultExGTest() // compare ExG Register bytes in form with the default for ExG Test
        {
            var arraysAreEqual = Enumerable.SequenceEqual(PConfiguration.ExgReg1UI, defaultExGTestReg1) && Enumerable.SequenceEqual(PConfiguration.ExgReg2UI, defaultExGTestReg2);
            return arraysAreEqual;
        }
        
        public int ConvertEXGReferenceValuetoSetting(String mode, int value)
        {

            if (mode.Equals("default")) //ECG enabled and EMG disabled
            {
                return 0;
            }
            else
            {
                if (value == 0 )
                {
                    return 0;
                }
                else
                {
                    if(mode.Equals("ECG")) //ECG enabled
                    {
                        return 13;
                    }
                    else //EMG enabled
                    {
                        return 3;
                    }
                }
            }
        }

        public int ConvertEXGLeadOffCurrenttoSetting(int value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value == 1)
            {
                return 4;
            }
            else if (value == 2)
            {
                return 8;
            }
            else
            {
                return 12;
            }
        }          

        public int ConvertEXGGainValuetoSetting(int value)
        {
            if (value == 6)
            {
                return 0;
            }
            else if (value == 1)
            {
                return 1;
            }
            else if (value == 2)
            {
                return 2;
            }
            else if (value == 3)
            {
                return 3;
            }
            else if (value == 4)
            {
                return 4;
            }
            else if (value == 8)
            {
                return 5;
            }
            else if (value == 12)
            {
                return 6;
            }
            else
            {
                return -1; // -1 means invalid value
            }
        }

        public int ConvertEXGSettingtoComboBoxIndex(int value)
        {
            if (value == 0)
            {
                return 4;
            }
            else if (value == 1)
            {
                return 0;
            }
            else if (value == 2)
            {
                return 1;
            }
            else if (value == 3)
            {
                return 2;
            }
            else if (value == 4)
            {
                return 3;
            }
            else if (value == 5)
            {
                return 5;
            }
            else if (value == 6)
            {
                return 6;
            }
            else
            {
                return 7; // -1 means invalid value
            }
        }

        public int ConvertLeadOffComparatorThresholdtoSetting (int value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value == 1)
            {
                return 32;
            }
            else if (value == 2)
            {
                return 64;
            }
            else if (value == 3)
            {
                return 96;
            }
            else if (value == 4)
            {
                return 128;
            }
            else if (value == 5)
            {
                return 160;
            }
            else if (value == 6)
            {
                return 192;
            }
            else
            {
                return 224;
            }
        }


        private void LoadExGConfigurationFromMainForm()
        {
            textBoxChip1Reg1.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[0] });
            textBoxChip1Reg2.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[1] });
            textBoxChip1Reg3.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[2] });
            textBoxChip1Reg4.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[3] });
            textBoxChip1Reg5.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[4] });
            textBoxChip1Reg6.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[5] });
            textBoxChip1Reg7.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[6] });
            textBoxChip1Reg8.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[7] });
            textBoxChip1Reg9.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[8] });
            textBoxChip1Reg10.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg1UI[9] });

            textBoxChip2Reg1.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[0] });
            textBoxChip2Reg2.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[1] });
            textBoxChip2Reg3.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[2] });
            textBoxChip2Reg4.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[3] });
            textBoxChip2Reg5.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[4] });
            textBoxChip2Reg6.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[5] });
            textBoxChip2Reg7.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[6] });
            textBoxChip2Reg8.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[7] });
            textBoxChip2Reg9.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[8] });
            textBoxChip2Reg10.Text = BitConverter.ToString(new byte[] { PConfiguration.ExgReg2UI[9] });

            if (IsDefaultExgTestSignalConfigurationEnabled(PConfiguration.ExgReg1UI, PConfiguration.ExgReg2UI))
            {
                labelEXG.Text = "Test Signal";
            }
            else if (IsDefaultECGConfigurationEnabled(PConfiguration.ExgReg1UI, PConfiguration.ExgReg2UI))
            {
                labelEXG.Text = "ECG";
            }
            else if (IsDefaultEMGConfigurationEnabled(PConfiguration.ExgReg1UI, PConfiguration.ExgReg2UI))
            {
                labelEXG.Text = "EMG";
            }
            else
            {
                labelEXG.Text = "Custom";
            }

            if (PConfiguration.PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                setExGUIElements();
                setExGRegBytesinForm();
            }
        }

        private void setExGUIElements()
        {
            // update lead-off detection mode combox, check byte 1 of both chips and byte 5 of 
            if (PConfiguration.PControlForm.ShimmerDevice.GetShimmerVersion() == (int)Shimmer.ShimmerVersion.SHIMMER3)
            {
                byte byte1exg1 = PConfiguration.ExgReg1UI[1];
                byte byte5exg1 = PConfiguration.ExgReg1UI[5];
                byte byte6exg1 = PConfiguration.ExgReg1UI[6];
                byte byte1exg2 = PConfiguration.ExgReg2UI[1];
                byte byte4exg2 = PConfiguration.ExgReg2UI[4];
                byte byte6exg2 = PConfiguration.ExgReg2UI[6];

                if (((byte1exg1 & 0x40) == 0) && ((byte1exg2 & 0x40) == 0) && ((byte5exg1 & 0x10) == 0) && ((byte6exg1 & 0x0F) == 0) && ((byte6exg2 & 0x0F) == 0) && (((byte4exg2 & 0x80) == 0) || ((byte4exg2 & 0x80) == 0X80)))
                {
                    comboBoxLeadOffDetection.SelectedIndex = 0;
                    comboBoxLeadOffDetection.Text = Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS[0];
                }
                else if (((byte1exg1 & 0x40) == 0x40) && ((byte1exg2 & 0x40) == 0x40) && ((byte5exg1 & 0x10) == 0x10) && ((byte6exg1 & 0x0F) == 0x07) && ((byte6exg2 & 0x0F) == 0x04) && ((byte4exg2 & 0x80) == 0))
                {
                    comboBoxLeadOffDetection.SelectedIndex = 1;
                    comboBoxLeadOffDetection.Text = Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS[1];
                }
                else
                {
                    comboBoxLeadOffDetection.SelectedIndex = 0;
                    comboBoxLeadOffDetection.Text = "Custom";
                }

                comboBoxExGReferenceElectrode.Items.Clear();
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked) // ECG enabled
                {
                    comboBoxExGReferenceElectrode.Items.AddRange(Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES);
                    comboBoxExGReferenceElectrode.Enabled = true;
                    comboBoxLeadOffDetection.Enabled = true;
                }
                else if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked) // EMG enabled
                {
                    comboBoxExGReferenceElectrode.Items.AddRange(Shimmer.LIST_OF_EXG_EMG_REFERENCE_ELECTRODES);
                    comboBoxExGReferenceElectrode.Enabled = true;
                    comboBoxLeadOffDetection.Enabled = false;
                }
                else
                {
                    comboBoxExGReferenceElectrode.Items.AddRange(Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES); // ECG and EMG disabled
                    comboBoxExGReferenceElectrode.Enabled = false;
                    comboBoxLeadOffDetection.Enabled = false;
                }

                // update lead-off current detection combo box (just checking byte 2 of ExG reg 1, byte 2 of ExG reg 2 should be the same!

                byte byte2exg1 = PConfiguration.ExgReg1UI[2];

                if ((byte2exg1 & 0x0C) == 0) // 6nA
                {
                    comboBoxExGLeadOffCurrent.SelectedIndex = 0;
                }
                else if ((byte2exg1 & 0x0C) == 4) // 22nA
                {
                    comboBoxExGLeadOffCurrent.SelectedIndex = 1;
                }
                else if ((byte2exg1 & 0x0C) == 8) // 6uA
                {
                    comboBoxExGLeadOffCurrent.SelectedIndex = 2;
                }
                else // 22 uA
                {
                    comboBoxExGLeadOffCurrent.SelectedIndex = 3;
                }

                // update lead-off comparator threshold box (just checking byte 2 of ExG reg 1, byte 2 of ExG reg 2 should be the same!

                byte2exg1 = PConfiguration.ExgReg1UI[2];

                if ((byte2exg1 & 0xE0) == 0) // Pos:95% - Neg:5%
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 0;
                }
                else if ((byte2exg1 & 0xE0) == 32) // Pos:92.5% - Neg:7.5% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 1;
                }
                else if ((byte2exg1 & 0xE0) == 64) // Pos:90% - Neg:10% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 2;
                }
                else if ((byte2exg1 & 0xE0) == 96) // Pos:87.5% - Neg:12.5% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 3;
                }
                else if ((byte2exg1 & 0xE0) == 128) // Pos:85% - Neg:15% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 4;
                }
                else if ((byte2exg1 & 0xE0) == 160) // Pos:80% - Neg:20% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 5;
                }
                else if ((byte2exg1 & 0xE0) == 192) // Pos:75% - Neg:25% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 6;
                }
                else // Pos:70% - Neg:30% 
                {
                    comboBoxLeadOffComparatorThreshold.SelectedIndex = 7;
                }

                // update reference electrode combo box

                byte5exg1 = PConfiguration.ExgReg1UI[5];

                if ((byte5exg1 & 0x0F) == 0) // reference electrode is lower 4 bits of byte 5 on ExG chip 1
                {
                    comboBoxExGReferenceElectrode.SelectedIndex = 0;
                    comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES[0];
                }
                else if ((byte5exg1 & 0x0F) == 13)
                {
                    comboBoxExGReferenceElectrode.SelectedIndex = 1;
                    if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked) // ECG enabled
                    {
                        comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES[1];
                    }
                    else
                    {
                        comboBoxExGReferenceElectrode.Text = "Custom";
                    }
                }

                else if ((byte5exg1 & 0x0F) == 3)
                {
                    comboBoxExGReferenceElectrode.SelectedIndex = 1;
                    if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked) // EMG enabled
                    {
                        comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_EMG_REFERENCE_ELECTRODES[1];
                    }
                    else
                    {
                        comboBoxExGReferenceElectrode.Text = "Custom";
                    }
                }

                else
                {
                    comboBoxExGReferenceElectrode.SelectedIndex = 0;
                    comboBoxExGReferenceElectrode.Text = "Custom";
                }

                if ((int)comboBoxLeadOffDetection.SelectedIndex == 0 || comboBoxLeadOffDetection.Text.Equals("Custom"))
                {
                    comboBoxExGLeadOffCurrent.Enabled = false;
                    comboBoxLeadOffComparatorThreshold.Enabled = false;
                }
                else
                {
                    comboBoxExGLeadOffCurrent.Enabled = true;
                    comboBoxLeadOffComparatorThreshold.Enabled = true;
                }

                int gainexg1ch1 = ((PConfiguration.ExgReg1UI[3] >> 4) & 7);
                int gainexg1ch2 = ((PConfiguration.ExgReg1UI[4] >> 4) & 7);
                int gainexg2ch1 = ((PConfiguration.ExgReg2UI[3] >> 4) & 7);
                int gainexg2ch2 = ((PConfiguration.ExgReg2UI[4] >> 4) & 7);

                if ((gainexg1ch1 == gainexg1ch2 && gainexg1ch2 == gainexg2ch1 && gainexg2ch1 == gainexg2ch2) && (gainexg1ch1 <= 6))
                {
                    PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedIndex = ConvertEXGSettingtoComboBoxIndex(gainexg1ch1);
                }
                else
                {
                    PConfiguration.userControlGeneralConfig1.comboBoxExgGain.Text = "Custom";
                }

                checkBoxDefaultECG.Checked = false;
                checkBoxDefaultEMG.Checked = false;
                checkBoxDefaultExGTest.Checked = false;

                if (isDefaultECG())
                {
                    checkBoxDefaultECG.Checked = true;
                }
                if (isDefaultEMG())
                {
                    checkBoxDefaultEMG.Checked = true;
                }
                if (isDefaultExGTest())
                {
                    checkBoxDefaultExGTest.Checked = true;
                }
            }
        }

        public void setExGRegBytesinForm() // reads the combobox settings (Reference Electrode, lead-off detection, lead-off current, threshold, gain) and sets the ExG Reg bytes
        {
            this.textBoxChip1Reg1.TextChanged -= new System.EventHandler(this.textBoxChip1Reg1_TextChanged);
            this.textBoxChip2Reg1.TextChanged -= new System.EventHandler(this.textBoxChip2Reg1_TextChanged);
            string sr = PConfiguration.userControlGeneralConfig1.comboBoxSamplingRate.SelectedItem.ToString();
            string subsr = sr.Substring(0, sr.Length - 2);
            double samplingRate = Double.Parse(subsr);
            int oversamplingRatio = 2;
            int gain = 0;
            int gainSetting = ConvertEXGGainValuetoSetting(gain);
            if (PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedItem.ToString().Equals("Custom")){
            

            } else {
                gain = (int)Double.Parse(PConfiguration.userControlGeneralConfig1.comboBoxExgGain.SelectedItem.ToString());
                gainSetting = ConvertEXGGainValuetoSetting(gain);
            }
            int referenceElectrode = (int)PConfiguration.userControlExgConfig1.comboBoxExGReferenceElectrode.SelectedIndex;
            
            String ExGMode;

            if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked)
            {
                ExGMode = "ECG";
            }
            else if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
            {
                ExGMode = "EMG";
            }
            else
            {
                ExGMode = "Default";
            }

            int referenceElectrodeSetting = ConvertEXGReferenceValuetoSetting(ExGMode, referenceElectrode);

            int leadOffCurrent = (int)PConfiguration.userControlExgConfig1.comboBoxExGLeadOffCurrent.SelectedIndex;
            int leadOffCurrentSetting = ConvertEXGLeadOffCurrenttoSetting(leadOffCurrent);

            int leadOffComparatorThreshold = (int)PConfiguration.userControlExgConfig1.comboBoxLeadOffComparatorThreshold.SelectedIndex;
            int leadOffComparatorThresholdSetting = ConvertLeadOffComparatorThresholdtoSetting(leadOffComparatorThreshold);

            int leadOffDetection = (int)comboBoxLeadOffDetection.SelectedIndex;

            int LeadOffDetectCurrent, LeadOffDetectComparators, LeadOffDetect_RLD_sense, LeadOffDetect_2P_1N_1P, LeadOffDetect_2P, LeadOffDetect_ECG, LeadOffDetect_EMG, RLD_Buffer_Power;

            if (samplingRate < 125)
            {
                oversamplingRatio = 0;
            }
            else if (samplingRate < 250)
            {
                oversamplingRatio = 1;
            }
            else if (samplingRate < 500)
            {
                oversamplingRatio = 2;
            }
            else if (samplingRate < 1000)
            {
                oversamplingRatio = 3;
            }
            else if (samplingRate < 2000)
            {
                oversamplingRatio = 4;
            }
            else if (samplingRate < 4000)
            {
                oversamplingRatio = 5;
            }
            else if (samplingRate < 8000)
            {
                oversamplingRatio = 6;
            }
            
            if (comboBoxLeadOffDetection.Text.Equals(Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS[0])) // Lead-Off Detection is OFF
            {
                
                LeadOffDetectCurrent = 0x00;
                LeadOffDetectComparators = 0x00;
                LeadOffDetect_RLD_sense = 0x00;
                LeadOffDetect_2P_1N_1P = 0x00;
                LeadOffDetect_2P = 0x00;
                LeadOffDetect_ECG = 0x00;
                LeadOffDetect_EMG = 0x80;
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
                {
                    RLD_Buffer_Power = 0x20;
                }
                else
                {
                    RLD_Buffer_Power = 0x00;
                }
            }
            else if (comboBoxLeadOffDetection.Text.Equals(Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS[1])) // DC Current Lead-Off Detection
            {
                LeadOffDetectCurrent = 0x00;
                LeadOffDetectComparators = 0x40;
                LeadOffDetect_RLD_sense = 0x10;
                LeadOffDetect_2P_1N_1P = 0x07;
                LeadOffDetect_2P = 0x04;
                LeadOffDetect_ECG = 0x00;
                LeadOffDetect_EMG = 0x00;
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
                {
                    RLD_Buffer_Power = 0x20;
                }
                else
                {
                    RLD_Buffer_Power = 0x00;
                }
            }
            else // custom bytes for lead off detection mode
            {
                LeadOffDetectCurrent = 0x00;
                LeadOffDetectComparators = 0x00;
                LeadOffDetect_RLD_sense = 0x00;
                LeadOffDetect_2P_1N_1P = 0x00;
                LeadOffDetect_2P = 0x00;
                LeadOffDetect_ECG = 0x00;
                LeadOffDetect_EMG = 0x80;
                RLD_Buffer_Power = 0x00;
            }

            //HEX:
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            string exg1Hex = BitConverter.ToString(exg1Reg);
            string[] exg1RegHex = exg1Hex.Split('-');
            //FILL EXG CONFIGURATIONS CHIP 1
            byte byte0exg1 = (byte)(((exg1Reg[0] >> 3) << 3) | oversamplingRatio);

            // set lead-off detection current, lead-off comparator threshold for chip1
            exg1Reg[2] = (byte)((exg1Reg[2] & 0x1F) | leadOffComparatorThresholdSetting);
            exg1Reg[2] = (byte)((exg1Reg[2] & 0xF3) | leadOffCurrentSetting);
            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                exg1Reg[2] = (byte)((exg1Reg[2] & 0xFE) | LeadOffDetectCurrent);
            }
            byte byte2exg1 = exg1Reg[2];
            byte byte3exg1 = exg1Reg[3];
            byte byte4exg1 = exg1Reg[4];
            // set gain for chip1
            if (!PConfiguration.userControlGeneralConfig1.comboBoxExgGain.Text.Equals("Custom"))
            {
                byte3exg1 = (byte)((exg1Reg[3] & 0x8F) | (gainSetting << 4));
                byte4exg1 = (byte)((exg1Reg[4] & 0x8F) | (gainSetting << 4));
            }

            // set reference electrode for chip1 (no reference electrode for chip2)
            if (!comboBoxExGReferenceElectrode.Text.Equals("Custom"))
            {
                exg1Reg[5] = (byte)((exg1Reg[5] & 0xF0) | (referenceElectrodeSetting));
                if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
                {
                    exg1Reg[5] = (byte)((exg1Reg[5] & 0xEF) | (LeadOffDetect_RLD_sense));
                }
                exg1Reg[5] = (byte)((exg1Reg[5]) | (RLD_Buffer_Power));
            }
            byte byte5exg1 = exg1Reg[5];

            byte byte1exg1 = exg1Reg[1];
            byte byte6exg1 = exg1Reg[6];

            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                byte1exg1 = (byte)((exg1Reg[1] & 0xBF) | (LeadOffDetectComparators)); ;
                byte6exg1 = (byte)((exg1Reg[6] & 0xF0) | (LeadOffDetect_2P_1N_1P));
            }

            textBoxChip1Reg1.Text = BitConverter.ToString(new byte[] { byte0exg1 });
            textBoxChip1Reg2.Text = BitConverter.ToString(new byte[] { byte1exg1 });
            textBoxChip1Reg3.Text = BitConverter.ToString(new byte[] { byte2exg1 });
            textBoxChip1Reg4.Text = BitConverter.ToString(new byte[] { byte3exg1 });
            textBoxChip1Reg5.Text = BitConverter.ToString(new byte[] { byte4exg1 });
            textBoxChip1Reg6.Text = BitConverter.ToString(new byte[] { byte5exg1 });
            textBoxChip1Reg7.Text = BitConverter.ToString(new byte[] { byte6exg1 });
            textBoxChip1Reg8.Text = exg1RegHex[7];
            textBoxChip1Reg9.Text = exg1RegHex[8];
            textBoxChip1Reg10.Text = exg1RegHex[9];

            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            string exg2Hex = BitConverter.ToString(exg2Reg);
            string[] exg2RegHex = exg2Hex.Split('-');
            //FILL EXG CONFIGURATIONS CHIP 2

            // set lead off detection current, lead-off comparator threshold for chip2
            exg2Reg[2] = (byte)((exg2Reg[2] & 0xF3) | leadOffCurrentSetting);
            exg2Reg[2] = (byte)((exg2Reg[2] & 0x1F) | leadOffComparatorThresholdSetting);
            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                exg2Reg[2] = (byte)((exg2Reg[2] & 0xFE) | (LeadOffDetectCurrent));
            }

            byte byte2exg2 = exg2Reg[2];
            byte byte3exg2 = exg2Reg[3];

            // set gain for chip2
            if (!PConfiguration.userControlGeneralConfig1.comboBoxExgGain.Text.Equals("Custom"))
            {
                byte3exg2 = (byte)((exg2Reg[3] & 0x8F) | (gainSetting << 4));
                exg2Reg[4] = (byte)((exg2Reg[4] & 0x8F) | (gainSetting << 4));
            }

            // set lead-off detection mode for chip2
            byte byte1exg2 = exg2Reg[1];
            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                byte1exg2 = (byte)((exg2Reg[1] & 0xBF) | (LeadOffDetectComparators));
            }
            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked) // ECG enabled
                {
                    exg2Reg[4] = (byte)((exg2Reg[4] & 0x7F) | (LeadOffDetect_ECG));
                }
                else if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked) // EMG enabled
                {
                    exg2Reg[4] = (byte)((exg2Reg[4] & 0x7F) | (LeadOffDetect_EMG));
                }
            }

            byte byte4exg2 = exg2Reg[4];
            byte byte6exg2 = exg2Reg[6];
            if (!comboBoxLeadOffDetection.Text.Equals("Custom"))
            {
                byte6exg2 = (byte)((exg2Reg[6] & 0xF0) | (LeadOffDetect_2P));
            }
            byte byte0exg2 = (byte)(((exg2Reg[0] >> 3) << 3) | oversamplingRatio);
            textBoxChip2Reg1.Text = BitConverter.ToString(new byte[] { byte0exg2 });
            textBoxChip2Reg2.Text = BitConverter.ToString(new byte[] { byte1exg2 });
            textBoxChip2Reg3.Text = BitConverter.ToString(new byte[] { byte2exg2 });
            textBoxChip2Reg4.Text = BitConverter.ToString(new byte[] { byte3exg2 });
            textBoxChip2Reg5.Text = BitConverter.ToString(new byte[] { byte4exg2 });
            textBoxChip2Reg6.Text = exg2RegHex[5];
            textBoxChip2Reg7.Text = BitConverter.ToString(new byte[] { byte6exg2 });
            textBoxChip2Reg8.Text = exg2RegHex[7];
            textBoxChip2Reg9.Text = exg2RegHex[8];
            textBoxChip2Reg10.Text = exg2RegHex[9];

            this.textBoxChip1Reg1.TextChanged += new System.EventHandler(this.textBoxChip1Reg1_TextChanged);
            this.textBoxChip1Reg2.TextChanged += new System.EventHandler(this.textBoxChip1Reg2_TextChanged);
        }

        public void ApplyExgConfigurations()
        {
            if (checkBoxNQFilter.Checked)
            {
                PConfiguration.PControlForm.EnableNQF = true;
            }
            else
            {
                PConfiguration.PControlForm.EnableNQF = false;
            }

            if (checkBoxHPF5.Checked)
            {
                PConfiguration.PControlForm.EnableHPF_5HZ = true;
                PConfiguration.PControlForm.EnableHPF_0_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_05HZ = false;
            }
            else if (checkBoxHPF0_5.Checked)
            {
                PConfiguration.PControlForm.EnableHPF_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_5HZ = true;
                PConfiguration.PControlForm.EnableHPF_0_05HZ = false;
            }
            else if (checkBoxHPF0_05.Checked)
            {
                PConfiguration.PControlForm.EnableHPF_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_05HZ = true;
            }
            else
            {
                PConfiguration.PControlForm.EnableHPF_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_5HZ = false;
                PConfiguration.PControlForm.EnableHPF_0_05HZ = false;
            }

            if (checkBoxBSF50.Checked)
            {
                PConfiguration.PControlForm.EnableBSF_49_51HZ = true;
                PConfiguration.PControlForm.EnableBSF_59_61HZ = false;
            }
            else if (checkBoxBSF60.Checked)
            {
                PConfiguration.PControlForm.EnableBSF_49_51HZ = false;
                PConfiguration.PControlForm.EnableBSF_59_61HZ = true;
            }
            else
            {
                PConfiguration.PControlForm.EnableBSF_49_51HZ = false;
                PConfiguration.PControlForm.EnableBSF_59_61HZ = false;
            }

            // Hex:
            try
            {
                Byte[] config1 = { (byte)Convert.ToInt32(textBoxChip1Reg1.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg2.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg3.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg4.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg5.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg6.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg7.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg8.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg9.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg10.Text, 16) };
                Byte[] config2 = { (byte)Convert.ToInt32(textBoxChip2Reg1.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg2.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg3.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg4.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg5.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg6.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg7.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg8.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg9.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg10.Text, 16) };
                PConfiguration.PControlForm.ShimmerDevice.WriteEXGConfigurations(config1, config2);
            }
            catch
            {
                MessageBox.Show("Invalid value found in EXG Chip register text box.", Control.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        /// <summary>
        /// This returns a false if 
        /// </summary>
        /// <returns></returns>
        public bool TestTextBoxExGConfigurations()
        {
            bool check = true;
            if (!CheckTextBoxIfValidHex(textBoxChip1Reg1))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg2))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg3))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg4))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg5))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg6))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg7))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg8))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg9))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip1Reg10))
            {
                check = false;
            }
            if (!CheckTextBoxIfValidHex(textBoxChip2Reg1))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg2))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg3))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg4))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg5))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg6))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg7))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg8))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg9))
            {
                check = false;
            }
            else if (!CheckTextBoxIfValidHex(textBoxChip2Reg10))
            {
                check = false;
            }
            return check;
        }

        public byte[] getByteEXG1FromForm()
        {
            Byte[] config1 = { (byte)Convert.ToInt32(textBoxChip1Reg1.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg2.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg3.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg4.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg5.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg6.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg7.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg8.Text, 16), (byte)Convert.ToInt32(textBoxChip1Reg9.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip1Reg10.Text, 16) };
            return config1;
        }

        public byte[] getByteEXG2FromForm()
        {

            Byte[] config2 = { (byte)Convert.ToInt32(textBoxChip2Reg1.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg2.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg3.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg4.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg5.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg6.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg7.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg8.Text, 16), (byte)Convert.ToInt32(textBoxChip2Reg9.Text, 16), 
                             (byte)Convert.ToInt32(textBoxChip2Reg10.Text, 16) };
            return config2;
        }

        private void textBoxChip1Reg1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void textBoxChip1Reg2_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[1] = (byte)Convert.ToInt32(textBoxChip1Reg2.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
            
        }

        private void textBoxChip1Reg3_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[2] = (byte)Convert.ToInt32(textBoxChip1Reg3.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
            
        }

        private void textBoxChip1Reg4_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[3] = (byte)Convert.ToInt32(textBoxChip1Reg4.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
            
        }

        private void textBoxChip1Reg5_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[4] = (byte)Convert.ToInt32(textBoxChip1Reg5.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
             
        }

        private void textBoxChip1Reg6_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[5] = (byte)Convert.ToInt32(textBoxChip1Reg6.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
            
        }

        private void textBoxChip1Reg7_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            exg1Reg[6] = (byte)Convert.ToInt32(textBoxChip1Reg7.Text, 16);
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGUIElements();
            
        }

        private void textBoxChip1Reg8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip1Reg9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip1Reg10_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBoxChip2Reg1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip2Reg2_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            exg2Reg[1] = (byte)Convert.ToInt32(textBoxChip2Reg2.Text, 16);
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGUIElements();
            
        }

        private void textBoxChip2Reg3_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            exg2Reg[2] = (byte)Convert.ToInt32(textBoxChip2Reg3.Text, 16);
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGUIElements();
            
        }

        private void textBoxChip2Reg4_TextChanged(object sender, EventArgs e)
        {
            
            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            exg2Reg[3] = (byte)Convert.ToInt32(textBoxChip2Reg4.Text, 16);
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGUIElements();
            
        }

        private void textBoxChip2Reg5_TextChanged(object sender, EventArgs e)
        {
           
            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            exg2Reg[4] = (byte)Convert.ToInt32(textBoxChip2Reg5.Text, 16);
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGUIElements();
            
        }

        private void textBoxChip2Reg6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip2Reg7_TextChanged(object sender, EventArgs e)
        {
            byte[] exg2Reg = PConfiguration.ExgReg2UI;
            exg2Reg[6] = (byte)Convert.ToInt32(textBoxChip2Reg7.Text, 16);
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGUIElements();
        }

        private void textBoxChip2Reg8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip2Reg9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxChip2Reg10_TextChanged(object sender, EventArgs e)
        {

        }
        
        private bool CheckTextBoxIfValidHex(object sender)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null || string.IsNullOrEmpty(textBox.Text))
            {
                string theText = textBox.Text;
                byte n;
                bool isByte = Byte.TryParse(theText, System.Globalization.NumberStyles.HexNumber, null, out n);
                if (!isByte)
                {
                    return false;
                }
            }
            return true;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            buttonApply.Text = "Applying";
            buttonApply.Enabled = false;
            ApplyExgConfigurations();
            PConfiguration.PControlForm.ShimmerDevice.SetConfigTime(Convert.ToInt32(PConfiguration.PControlForm.ShimmerDevice.SystemTime2Config()));
            PConfiguration.PControlForm.ShimmerDevice.WriteConfigTime();
            //PConfiguration.PControlForm.ShimmerDevice.WriteSdConfigFile();
            buttonApply.Text = "Apply";
            buttonApply.Enabled = true;

            PConfiguration.PControlForm.AppendTextBox("Configuration done.");
            MessageBox.Show("Configurations changed.", ShimmerSDBT.AppNameCapture,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBoxTestSignal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxHPF0_05_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNQFilter_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxBSF60_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void ForceExGConfigurationUpdate()
        {
            //if (changeFromOtherWindow)
            {
                //SetExgGainCmbToCurrentShimmerSettings();
                LoadExGConfigurationFromMainForm();

            }
        }

        /// <summary>
        /// This can be used to check the registers on the ExG Daughter board and determine whether it is using default ECG configurations
        /// </summary>
        /// <returns>Returns true if defaul ECG configurations is being used</returns>
        public bool IsDefaultExgTestSignalConfigurationEnabled(byte[] Exg1RegArray, byte[] Exg2RegArray)
        {
            bool isUsing = false;
            if (((Exg1RegArray[3] & 15) == 5) && ((Exg1RegArray[4] & 15) == 5) && ((Exg2RegArray[3] & 15) == 5) && ((Exg2RegArray[4] & 15) == 5))
            {
                isUsing = true;
            }
            return isUsing;
        }

        /// <summary>
        /// This can be used to check the registers on the ExG Daughter board and determine whether it is using default ECG configurations
        /// </summary>
        /// <returns>Returns true if defaul ECG configurations is being used</returns>
        public bool IsDefaultECGConfigurationEnabled(byte[] Exg1RegArray, byte[] Exg2RegArray)
        {
            bool isUsing = false;
            if (((Exg1RegArray[3] & 15) == 0) && ((Exg1RegArray[4] & 15) == 0) && ((Exg2RegArray[3] & 15) == 0) && ((Exg2RegArray[4] & 15) == 7))
            {
                isUsing = true;
            }
            return isUsing;
        }
        /// <summary>
        /// This can be used to check the registers on the ExG Daughter board and determine whether it is using default EMG configurations
        /// </summary>
        /// <returns>Returns true if defaul EMG configurations is being used</returns>
        public bool IsDefaultEMGConfigurationEnabled(byte[] Exg1RegArray, byte[] Exg2RegArray)
        {
            bool isUsing = false;
            if (((Exg1RegArray[3] & 15) == 9) && ((Exg1RegArray[4] & 15) == 0) && ((Exg2RegArray[3] & 15) == 1) && ((Exg2RegArray[4] & 15) == 1))
            {
                isUsing = true;
            }

            return isUsing;
        }

        private void groupBoxSettings_Enter(object sender, EventArgs e)
        {

        }

        private void labelEXG_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxExg1Reg1_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg2_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg3_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg4_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg5_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg6_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg7_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg8_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg9_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg1Reg10_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg2_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg1_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg3_OnLeave(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg4(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg5(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg6(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg7(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg8(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg9(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void TextBoxExg2Reg10(object sender, EventArgs e)
        {
            //CheckTextBoxIfValidHex(sender);
        }

        private void checkBoxDefaultECG_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxDefaultEMG_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxDefaultExGTest_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxExGReferenceElectrode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            byte[] exg2Reg = PConfiguration.ExgReg2UI;

            int referenceElectrode = (int)PConfiguration.userControlExgConfig1.comboBoxExGReferenceElectrode.SelectedIndex;
       
            String ExGMode;
            if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked)
            {
                ExGMode = "ECG";
                comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES[referenceElectrode];
            }
            else if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
            {
                ExGMode = "EMG";
                comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_EMG_REFERENCE_ELECTRODES[referenceElectrode];
            }
            else
            {
                ExGMode = "Default";
                comboBoxExGReferenceElectrode.Text = Shimmer.LIST_OF_EXG_ECG_REFERENCE_ELECTRODES[referenceElectrode];
            }

            int referenceElectrodeSetting = ConvertEXGReferenceValuetoSetting(ExGMode, referenceElectrode);
            // set reference electrode for chip1 (no reference electrode for chip2)
            byte byte5exg1 = (byte)((exg1Reg[5] & 0xF0) | (referenceElectrodeSetting));
            exg1Reg[5] = byte5exg1;
            PConfiguration.ExgReg1UI = exg1Reg;
            setExGRegBytesinForm();
        }

        private void comboBoxExGLeadOffCurrent_SelectionChangeCommitted(object sender, EventArgs e)
        {
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            byte[] exg2Reg = PConfiguration.ExgReg2UI;

            int leadOffCurrent = (int)PConfiguration.userControlExgConfig1.comboBoxExGLeadOffCurrent.SelectedIndex;
            int leadOffCurrentSetting = ConvertEXGLeadOffCurrenttoSetting(leadOffCurrent);

            byte byte2exg1 = (byte)((exg1Reg[2] & 0xF3) | leadOffCurrentSetting);
            byte byte2exg2 = (byte)((exg2Reg[2] & 0xF3) | leadOffCurrentSetting);

            exg1Reg[2] = byte2exg1;
            exg2Reg[2] = byte2exg2;

            PConfiguration.ExgReg1UI = exg1Reg;
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGRegBytesinForm();
        }

        private void comboBoxLeadOffComparatorThreshold_SelectionChangeCommitted(object sender, EventArgs e)
        {
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            byte[] exg2Reg = PConfiguration.ExgReg2UI;

            int leadOffComparatorThreshold = (int)PConfiguration.userControlExgConfig1.comboBoxLeadOffComparatorThreshold.SelectedIndex;
            int leadOffComparatorThresholdSetting = ConvertLeadOffComparatorThresholdtoSetting(leadOffComparatorThreshold);

            byte byte2exg1 = (byte)((exg1Reg[2] & 0x1F) | leadOffComparatorThresholdSetting);
            byte byte2exg2 = (byte)((exg2Reg[2] & 0x1F) | leadOffComparatorThresholdSetting);

            exg1Reg[2] = byte2exg1;
            exg2Reg[2] = byte2exg2;

            PConfiguration.ExgReg1UI = exg1Reg;
            PConfiguration.ExgReg2UI = exg2Reg;
            setExGRegBytesinForm();
        }

        private void comboBoxLeadOffDetection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            byte[] exg1Reg = PConfiguration.ExgReg1UI;
            byte[] exg2Reg = PConfiguration.ExgReg2UI;

            int leadOffDetection = (int)comboBoxLeadOffDetection.SelectedIndex;
            int LeadOffDetectCurrent, LeadOffDetectComparators, LeadOffDetect_RLD_sense, LeadOffDetect_2P_1N_1P, LeadOffDetect_2P, LeadOffDetect_ECG, LeadOffDetect_EMG, RLD_Buffer_Power;

            if (leadOffDetection == 0) // Lead-Off Detection is OFF
            {
                LeadOffDetectCurrent = 0x00;
                LeadOffDetectComparators = 0x00;
                LeadOffDetect_RLD_sense = 0x00;
                LeadOffDetect_2P_1N_1P = 0x00;
                LeadOffDetect_2P = 0x00;
                LeadOffDetect_ECG = 0x00;
                LeadOffDetect_EMG = 0x80;
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
                {
                    RLD_Buffer_Power = 0x20;
                }
                else
                {
                    RLD_Buffer_Power = 0x00;
                }
            }
            else // DC Current Lead-Off Detection
            {
                LeadOffDetectCurrent = 0x00;
                LeadOffDetectComparators = 0x40;
                LeadOffDetect_RLD_sense = 0x10;
                LeadOffDetect_2P_1N_1P = 0x07;
                LeadOffDetect_2P = 0x04;
                LeadOffDetect_ECG = 0x00;
                LeadOffDetect_EMG = 0x00;
                if (PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked)
                {
                    RLD_Buffer_Power = 0x20;
                }
                else
                {
                    RLD_Buffer_Power = 0x00;
                }
                comboBoxExGLeadOffCurrent.SelectedIndex = 1;
                comboBoxLeadOffComparatorThreshold.SelectedIndex = 2;
            }

            comboBoxLeadOffDetection.Text = (Shimmer.LIST_OF_EXG_LEAD_OFF_DETECTION_OPTIONS[leadOffDetection]);

            byte byte1exg1 = (byte)((exg1Reg[1] & 0xBF) | (LeadOffDetectComparators));
            byte byte1exg2 = (byte)((exg2Reg[1] & 0xBF) | (LeadOffDetectComparators));
            byte byte2exg1 = (byte)((exg1Reg[2] & 0xFE) | (LeadOffDetectCurrent));
            byte byte2exg2 = (byte)((exg2Reg[2] & 0xFE) | (LeadOffDetectCurrent));

            byte byte4exg2 = exg2Reg[4];

            if (PConfiguration.userControlGeneralConfig1.checkBoxSensor15.Checked) // ECG enabled
            {
                byte4exg2 = (byte)((exg2Reg[4] & 0x7F) | (LeadOffDetect_ECG));
            }
            else if(PConfiguration.userControlGeneralConfig1.checkBoxSensor16.Checked) // EMG enabled
            {
                byte4exg2 = (byte)((exg2Reg[4] & 0x7F) | (LeadOffDetect_EMG)); 
            }
            byte byte5exg1 = (byte)((exg1Reg[5] & 0xEF) | (LeadOffDetect_RLD_sense) | RLD_Buffer_Power); // (0x20) set bit 5 byte 5 high enabled RLD buffer power
            byte byte6exg1 = (byte)((exg1Reg[6] & 0xF0) | (LeadOffDetect_2P_1N_1P));
            byte byte6exg2 = (byte)((exg2Reg[6] & 0xF0) | (LeadOffDetect_2P));

            exg1Reg[1] = byte1exg1;
            exg1Reg[2] = byte2exg1;
            exg1Reg[5] = byte5exg1;
            exg1Reg[6] = byte6exg1;

            exg2Reg[1] = byte1exg2;
            exg2Reg[2] = byte2exg2;
            exg2Reg[4] = byte4exg2;
            exg2Reg[6] = byte6exg2;

            PConfiguration.ExgReg1UI = exg1Reg;
            PConfiguration.ExgReg2UI = exg2Reg;

            setExGRegBytesinForm();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxExGReferenceElectrode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
  
    }
}
