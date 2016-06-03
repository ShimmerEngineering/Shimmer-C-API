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
    public partial class ExgConfigure : UserControl
    {
        public Control PControlForm;

        public ExgConfigure()
        {
            InitializeComponent();
        }

        public ExgConfigure(Control controlForm)
            : this()
        {
            PControlForm = controlForm;
        }

        private void ExgConfigure_Load(object sender, EventArgs e)
        {
            checkBoxHPF0_05.Enabled = true;
            checkBoxHPF0_5.Enabled = true;
            checkBoxHPF5.Enabled = true;
            checkBoxBSF50.Enabled = true;
            checkBoxBSF60.Enabled = true;
            checkBoxDefaultECG.Enabled = true;
            checkBoxDefaultEMG.Enabled = true;
            checkBoxTestSignal.Enabled = true;

            comboBoxChip1Channel1Gain.Enabled = true;
            comboBoxChip1Channel2Gain.Enabled = true;
            comboBoxChip2Channel1Gain.Enabled = true;
            comboBoxChip2Channel2Gain.Enabled = true;
            comboBoxChip1Channel1Gain.Items.AddRange(Shimmer.LIST_OF_EXG_GAINS_SHIMMER3);
            comboBoxChip1Channel2Gain.Items.AddRange(Shimmer.LIST_OF_EXG_GAINS_SHIMMER3);
            comboBoxChip2Channel1Gain.Items.AddRange(Shimmer.LIST_OF_EXG_GAINS_SHIMMER3);
            comboBoxChip2Channel2Gain.Items.AddRange(Shimmer.LIST_OF_EXG_GAINS_SHIMMER3);

            byte[] exg1Reg = PControlForm.shimmer.GetEXG1RegisterContents();
            textBoxChip1Reg1.Text = exg1Reg[0].ToString();
            textBoxChip1Reg2.Text = exg1Reg[1].ToString();
            textBoxChip1Reg3.Text = exg1Reg[2].ToString();
            textBoxChip1Reg4.Text = exg1Reg[3].ToString();
            textBoxChip1Reg5.Text = exg1Reg[4].ToString();
            textBoxChip1Reg6.Text = exg1Reg[5].ToString();
            textBoxChip1Reg7.Text = exg1Reg[6].ToString();
            textBoxChip1Reg8.Text = exg1Reg[7].ToString();
            textBoxChip1Reg9.Text = exg1Reg[8].ToString();
            textBoxChip1Reg10.Text = exg1Reg[9].ToString();
            byte[] exg2Reg = PControlForm.shimmer.GetEXG2RegisterContents();
            textBoxChip2Reg1.Text = exg2Reg[0].ToString();
            textBoxChip2Reg2.Text = exg2Reg[1].ToString();
            textBoxChip2Reg3.Text = exg2Reg[2].ToString();
            textBoxChip2Reg4.Text = exg2Reg[3].ToString();
            textBoxChip2Reg5.Text = exg2Reg[4].ToString();
            textBoxChip2Reg6.Text = exg2Reg[5].ToString();
            textBoxChip2Reg7.Text = exg2Reg[6].ToString();
            textBoxChip2Reg8.Text = exg2Reg[7].ToString();
            textBoxChip2Reg9.Text = exg2Reg[8].ToString();
            textBoxChip2Reg10.Text = exg2Reg[9].ToString();

            //ECG
            //textBoxEXGChip1Reg1.Text = "2-160-16-64-64-45-0-0-2-3";
            //textBoxEXGChip2Reg1.Text = "2-160-16-64-71-0-0-0-2-1";
            if (PControlForm.shimmer.IsDefaultECGConfigurationEnabled())
            {
                checkBoxDefaultECG.Checked = true;
            }
            //EMG
            //textBoxEXGChip1Reg1.Text = "2-160-16-105-96-0-0-0-2-3";
            //textBoxEXGChip2Reg1.Text = "2-160-16-129-129-0-0-0-2-1";
            else if (PControlForm.shimmer.IsDefaultEMGConfigurationEnabled())
            {
                checkBoxDefaultEMG.Checked = true;
            }
            //Test Signal
            //byte[] reg1 = { 2, (byte)163, 16, 5, 5, 0, 0, 0, 2, 1 };
            //byte[] reg2 = { 2, (byte)163, 16, 5, 5, 0, 0, 0, 2, 1 };
            else if (textBoxChip1Reg1.Text.Equals("2") && textBoxChip1Reg2.Text.Equals("163") && textBoxChip1Reg3.Text.Equals("16") && textBoxChip1Reg4.Text.Equals("5") && textBoxChip1Reg5.Text.Equals("5") && textBoxChip1Reg6.Text.Equals("0") && textBoxChip1Reg7.Text.Equals("0") && textBoxChip1Reg8.Text.Equals("0") && textBoxChip1Reg9.Text.Equals("2") && textBoxChip1Reg10.Text.Equals("1")
                && textBoxChip2Reg1.Text.Equals("2") && textBoxChip2Reg2.Text.Equals("163") && textBoxChip2Reg3.Text.Equals("16") && textBoxChip2Reg4.Text.Equals("5") && textBoxChip2Reg5.Text.Equals("5") && textBoxChip2Reg6.Text.Equals("0") && textBoxChip2Reg7.Text.Equals("0") && textBoxChip2Reg8.Text.Equals("0") && textBoxChip2Reg9.Text.Equals("2") && textBoxChip2Reg10.Text.Equals("1"))
            {
                checkBoxTestSignal.Checked = true;
            }
            else
            {
                checkBoxDefaultECG.Checked = false;
                checkBoxDefaultEMG.Checked = false;
                checkBoxTestSignal.Checked = false;
            }

            SetExgGainCmbToCurrentSettings();

            checkBoxHPF0_05.Checked = false;
            checkBoxHPF0_5.Checked = false;
            checkBoxHPF5.Checked = false;
            checkBoxBSF50.Checked = false;
            checkBoxBSF60.Checked = false;

            if (PControlForm.EnableHPF_0_05HZ == true)
            {
                checkBoxHPF0_05.Checked = true;
                checkBoxHPF0_5.Checked = false;
                checkBoxHPF5.Checked = false;
            }
            else if (PControlForm.EnableHPF_0_5HZ == true)
            {
                checkBoxHPF0_05.Checked = false;
                checkBoxHPF0_5.Checked = true;
                checkBoxHPF5.Checked = false;
            }
            else if (PControlForm.EnableHPF_5HZ == true)
            {
                checkBoxHPF0_05.Checked = false;
                checkBoxHPF0_5.Checked = false;
                checkBoxHPF5.Checked = true;
            }

            if (PControlForm.EnableBSF_49_51HZ == true)
            {
                checkBoxBSF50.Checked = true;
                checkBoxBSF60.Checked = false;
            }
            else if (PControlForm.EnableBSF_59_61HZ == true)
            {
                checkBoxBSF50.Checked = false;
                checkBoxBSF60.Checked = true;
            }
        }

        private void SetExgGainCmbToCurrentSettings()
        {
            int exgGain1 = (PControlForm.shimmer.GetEXG1RegisterContents()[3] >> 4) & 7;
            if (exgGain1 == 0)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 4;
            }
            else if (exgGain1 == 1)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 0;
            }
            else if (exgGain1 == 2)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 1;
            }
            else if (exgGain1 == 3)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 2;
            }
            else if (exgGain1 == 4)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 3;
            }
            else if (exgGain1 == 5)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 5;
            }
            else if (exgGain1 == 6)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 6;
            }

            int exgGain2 = (PControlForm.shimmer.GetEXG1RegisterContents()[4] >> 4) & 7;
            if (exgGain2 == 0)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 4;
            }
            else if (exgGain2 == 1)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 0;
            }
            else if (exgGain2 == 2)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 1;
            }
            else if (exgGain2 == 3)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 2;
            }
            else if (exgGain2 == 4)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 3;
            }
            else if (exgGain2 == 5)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 5;
            }
            else if (exgGain2 == 6)
            {
                comboBoxChip1Channel2Gain.SelectedIndex = 6;
            }

            int exgGain3 = (PControlForm.shimmer.GetEXG2RegisterContents()[3] >> 4) & 7;
            if (exgGain3 == 0)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 4;
            }
            else if (exgGain3 == 1)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 0;
            }
            else if (exgGain3 == 2)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 1;
            }
            else if (exgGain3 == 3)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 2;
            }
            else if (exgGain3 == 4)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 3;
            }
            else if (exgGain3 == 5)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 5;
            }
            else if (exgGain3 == 6)
            {
                comboBoxChip2Channel1Gain.SelectedIndex = 6;
            }

            int exgGain4 = (PControlForm.shimmer.GetEXG2RegisterContents()[4] >> 4) & 7;
            if (exgGain4 == 0)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 4;

            }
            else if (exgGain4 == 1)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 0;
            }
            else if (exgGain4 == 2)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 1;
            }
            else if (exgGain4 == 3)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 2;
            }
            else if (exgGain4 == 4)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 3;
            }
            else if (exgGain4 == 5)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 5;
            }
            else if (exgGain4 == 6)
            {
                comboBoxChip2Channel2Gain.SelectedIndex = 6;
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
                checkBoxBSF60.Checked = false;
            }
        }

        private void checkBoxBSF60_Click(object sender, EventArgs e)
        {
            if (checkBoxBSF60.Checked)
            {
                checkBoxBSF50.Checked = false;
            }
        }

        private void checkBoxDefaultECG_Click(object sender, EventArgs e)
        {
            if (checkBoxDefaultECG.Checked)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 3;
                comboBoxChip1Channel2Gain.SelectedIndex = 3;
                comboBoxChip2Channel1Gain.SelectedIndex = 3;
                comboBoxChip2Channel2Gain.SelectedIndex = 3;

                checkBoxDefaultEMG.Checked = false;
                checkBoxTestSignal.Checked = false;

                ConfigChanges();    //called last
            }
            else
            {
                if ((!checkBoxDefaultEMG.Checked) && (!checkBoxTestSignal.Checked))
                {
                    ConfigChanges();
                }
            }
        }

        private void checkBoxDefaultEMG_Click(object sender, EventArgs e)
        {
            if (checkBoxDefaultEMG.Checked)
            {
                comboBoxChip1Channel1Gain.SelectedIndex = 6;
                comboBoxChip1Channel2Gain.SelectedIndex = 6;
                comboBoxChip2Channel1Gain.SelectedIndex = 4;
                comboBoxChip2Channel2Gain.SelectedIndex = 4;

                checkBoxDefaultECG.Checked = false;
                checkBoxTestSignal.Checked = false;
                ConfigChanges(); // called last
            }
            else
            {
                if ((!checkBoxDefaultECG.Checked) && (!checkBoxTestSignal.Checked))
                {
                    ConfigChanges();
                }
            }
        }

        private void checkBoxTestSignal_Click(object sender, EventArgs e)
        {
            if (checkBoxTestSignal.Checked)
            {
                checkBoxDefaultECG.Checked = false;
                checkBoxDefaultEMG.Checked = false;
                ConfigChanges();        //called last
            }
            else
            {
                if ((!checkBoxDefaultECG.Checked) && (!checkBoxDefaultEMG.Checked))
                {
                    ConfigChanges();
                }
            }
        }

        private void comboBoxChip1Channel1Gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte exgGain1 = Byte.Parse(textBoxChip1Reg4.Text);
            exgGain1 = (byte)(exgGain1 & 143);
            if (comboBoxChip1Channel1Gain.SelectedIndex == 0)
            {
                exgGain1 = (byte)(exgGain1 | 0x10);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 1)
            {
                exgGain1 = (byte)(exgGain1 | 0x20);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 2)
            {
                exgGain1 = (byte)(exgGain1 | 0x30);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 3)
            {
                exgGain1 = (byte)(exgGain1 | 0x40);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 4)
            {
                exgGain1 = (byte)(exgGain1 | 0x00);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 5)
            {
                exgGain1 = (byte)(exgGain1 | 0x50);
            }
            else if (comboBoxChip1Channel1Gain.SelectedIndex == 6)
            {
                exgGain1 = (byte)(exgGain1 | 0x60);
            }
            textBoxChip1Reg4.Text = ((int)(exgGain1)).ToString();
        }

        private void comboBoxChip1Channel2Gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte exgGain2 = Byte.Parse(textBoxChip1Reg5.Text);
            exgGain2 = (byte)(exgGain2 & 143);
            if (comboBoxChip1Channel2Gain.SelectedIndex == 0)
            {
                exgGain2 = (byte)(exgGain2 | 0x10);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 1)
            {
                exgGain2 = (byte)(exgGain2 | 0x20);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 2)
            {
                exgGain2 = (byte)(exgGain2 | 0x30);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 3)
            {
                exgGain2 = (byte)(exgGain2 | 0x40);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 4)
            {
                exgGain2 = (byte)(exgGain2 | 0x00);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 5)
            {
                exgGain2 = (byte)(exgGain2 | 0x50);
            }
            else if (comboBoxChip1Channel2Gain.SelectedIndex == 6)
            {
                exgGain2 = (byte)(exgGain2 | 0x60);
            }
            textBoxChip1Reg5.Text = ((int)(exgGain2)).ToString();
        }

        private void comboBoxChip2Channel1Gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte exgGain3 = Byte.Parse(textBoxChip2Reg4.Text);
            exgGain3 = (byte)(exgGain3 & 143);
            if (comboBoxChip2Channel1Gain.SelectedIndex == 0)
            {
                exgGain3 = (byte)(exgGain3 | 0x10);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 1)
            {
                exgGain3 = (byte)(exgGain3 | 0x20);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 2)
            {
                exgGain3 = (byte)(exgGain3 | 0x30);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 3)
            {
                exgGain3 = (byte)(exgGain3 | 0x40);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 4)
            {
                exgGain3 = (byte)(exgGain3 | 0x00);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 5)
            {
                exgGain3 = (byte)(exgGain3 | 0x50);
            }
            else if (comboBoxChip2Channel1Gain.SelectedIndex == 6)
            {
                exgGain3 = (byte)(exgGain3 | 0x60);
            }
            textBoxChip2Reg4.Text = ((int)(exgGain3)).ToString();
        }

        private void comboBoxChip2Channel2Gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte exgGain4 = Byte.Parse(textBoxChip2Reg5.Text);
            exgGain4 = (byte)(exgGain4 & 143);
            if (comboBoxChip2Channel2Gain.SelectedIndex == 0)
            {
                exgGain4 = (byte)(exgGain4 | 0x10);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 1)
            {
                exgGain4 = (byte)(exgGain4 | 0x20);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 2)
            {
                exgGain4 = (byte)(exgGain4 | 0x30);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 3)
            {
                exgGain4 = (byte)(exgGain4 | 0x40);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 4)
            {
                exgGain4 = (byte)(exgGain4 | 0x00);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 5)
            {
                exgGain4 = (byte)(exgGain4 | 0x50);
            }
            else if (comboBoxChip2Channel2Gain.SelectedIndex == 6)
            {
                exgGain4 = (byte)(exgGain4 | 0x60);
            }
            textBoxChip2Reg5.Text = ((int)(exgGain4)).ToString();
        }

        private void ConfigChanges()
        {
            if (checkBoxDefaultECG.Checked == true)
            {
                //textBoxEXGChip1Reg1.Text = "2-160-16-64-64-45-0-0-2-3";
                //textBoxEXGChip2Reg1.Text = "2-160-16-64-71-0-0-0-2-1";

                textBoxChip1Reg1.Text = "2";
                textBoxChip1Reg2.Text = "160";
                textBoxChip1Reg3.Text = "16";
                textBoxChip1Reg4.Text = "64";
                textBoxChip1Reg5.Text = "64";
                textBoxChip1Reg6.Text = "45";
                textBoxChip1Reg7.Text = "0";
                textBoxChip1Reg8.Text = "0";
                textBoxChip1Reg9.Text = "2";
                textBoxChip1Reg10.Text = "3";

                textBoxChip2Reg1.Text = "2";
                textBoxChip2Reg2.Text = "160";
                textBoxChip2Reg3.Text = "16";
                textBoxChip2Reg4.Text = "64";
                textBoxChip2Reg5.Text = "71";
                textBoxChip2Reg6.Text = "0";
                textBoxChip2Reg7.Text = "0";
                textBoxChip2Reg8.Text = "0";
                textBoxChip2Reg9.Text = "2";
                textBoxChip2Reg10.Text = "1";
            }
            else if (checkBoxDefaultEMG.Checked == true)
            {
                //textBoxEXGChip1Reg1.Text = "2-160-16-105-96-0-0-0-2-3";
                //textBoxEXGChip2Reg1.Text = "2-160-16-129-129-0-0-0-2-1";

                textBoxChip1Reg1.Text = "2";
                textBoxChip1Reg2.Text = "160";
                textBoxChip1Reg3.Text = "16";
                textBoxChip1Reg4.Text = "105";
                textBoxChip1Reg5.Text = "96";
                textBoxChip1Reg6.Text = "0";
                textBoxChip1Reg7.Text = "0";
                textBoxChip1Reg8.Text = "0";
                textBoxChip1Reg9.Text = "2";
                textBoxChip1Reg10.Text = "3";

                textBoxChip2Reg1.Text = "2";
                textBoxChip2Reg2.Text = "160";
                textBoxChip2Reg3.Text = "16";
                textBoxChip2Reg4.Text = "129";
                textBoxChip2Reg5.Text = "129";
                textBoxChip2Reg6.Text = "0";
                textBoxChip2Reg7.Text = "0";
                textBoxChip2Reg8.Text = "0";
                textBoxChip2Reg9.Text = "2";
                textBoxChip2Reg10.Text = "1";
            }
            else if (checkBoxTestSignal.Checked == true)
            {
                //byte[] reg1 = { 2, (byte)163, 16, 5, 5, 0, 0, 0, 2, 1 };
                //byte[] reg2 = { 2, (byte)163, 16, 5, 5, 0, 0, 0, 2, 1 };

                textBoxChip1Reg1.Text = "2";
                textBoxChip1Reg2.Text = "163";
                textBoxChip1Reg3.Text = "16";
                textBoxChip1Reg4.Text = "5";
                textBoxChip1Reg5.Text = "5";
                textBoxChip1Reg6.Text = "0";
                textBoxChip1Reg7.Text = "0";
                textBoxChip1Reg8.Text = "0";
                textBoxChip1Reg9.Text = "2";
                textBoxChip1Reg10.Text = "1";

                textBoxChip2Reg1.Text = "2";
                textBoxChip2Reg2.Text = "163";
                textBoxChip2Reg3.Text = "16";
                textBoxChip2Reg4.Text = "5";
                textBoxChip2Reg5.Text = "5";
                textBoxChip2Reg6.Text = "0";
                textBoxChip2Reg7.Text = "0";
                textBoxChip2Reg8.Text = "0";
                textBoxChip2Reg9.Text = "2";
                textBoxChip2Reg10.Text = "1";
            }
            else
            {
                //FILL EXG CONFIGURATIONS CHIP 1
                textBoxChip1Reg1.Text = PControlForm.shimmer.GetEXG1RegisterByte(0).ToString();
                textBoxChip1Reg2.Text = PControlForm.shimmer.GetEXG1RegisterByte(1).ToString();
                textBoxChip1Reg3.Text = PControlForm.shimmer.GetEXG1RegisterByte(2).ToString();
                textBoxChip1Reg4.Text = PControlForm.shimmer.GetEXG1RegisterByte(3).ToString();
                textBoxChip1Reg5.Text = PControlForm.shimmer.GetEXG1RegisterByte(4).ToString();
                textBoxChip1Reg6.Text = PControlForm.shimmer.GetEXG1RegisterByte(5).ToString();
                textBoxChip1Reg7.Text = PControlForm.shimmer.GetEXG1RegisterByte(6).ToString();
                textBoxChip1Reg8.Text = PControlForm.shimmer.GetEXG1RegisterByte(7).ToString();
                textBoxChip1Reg9.Text = PControlForm.shimmer.GetEXG1RegisterByte(8).ToString();
                textBoxChip1Reg10.Text = PControlForm.shimmer.GetEXG1RegisterByte(9).ToString();

                //FILL EXG CONFIGURATIONS CHIP2
                textBoxChip2Reg1.Text = PControlForm.shimmer.GetEXG2RegisterByte(0).ToString();
                textBoxChip2Reg2.Text = PControlForm.shimmer.GetEXG2RegisterByte(1).ToString();
                textBoxChip2Reg3.Text = PControlForm.shimmer.GetEXG2RegisterByte(2).ToString();
                textBoxChip2Reg4.Text = PControlForm.shimmer.GetEXG2RegisterByte(3).ToString();
                textBoxChip2Reg5.Text = PControlForm.shimmer.GetEXG2RegisterByte(4).ToString();
                textBoxChip2Reg6.Text = PControlForm.shimmer.GetEXG2RegisterByte(5).ToString();
                textBoxChip2Reg7.Text = PControlForm.shimmer.GetEXG2RegisterByte(6).ToString();
                textBoxChip2Reg8.Text = PControlForm.shimmer.GetEXG2RegisterByte(7).ToString();
                textBoxChip2Reg9.Text = PControlForm.shimmer.GetEXG2RegisterByte(8).ToString();
                textBoxChip2Reg10.Text = PControlForm.shimmer.GetEXG2RegisterByte(9).ToString();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

            
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (checkBoxHPF5.Checked)
            {
                PControlForm.EnableHPF_5HZ = true;
                PControlForm.EnableHPF_0_5HZ = false;
                PControlForm.EnableHPF_0_05HZ = false;
                if (PControlForm.shimmer.GetSamplingRate() < 50)
                {
                    MessageBox.Show("High Pass Filter only valid for the following frequencies, 51.2Hz, 102.4Hz, 204.8Hz, 256Hz, 512Hz, 1024Hz", 
                        Control.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (checkBoxHPF0_5.Checked)
            {
                PControlForm.EnableHPF_5HZ = false;
                PControlForm.EnableHPF_0_5HZ = true;
                PControlForm.EnableHPF_0_05HZ = false;
                if (PControlForm.shimmer.GetSamplingRate() < 50)
                {
                    MessageBox.Show("High Pass Filter only valid for the following frequencies, 51.2Hz, 102.4Hz, 204.8Hz, 256Hz, 512Hz, 1024Hz", 
                        Control.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (checkBoxHPF0_05.Checked)
            {
                PControlForm.EnableHPF_5HZ = false;
                PControlForm.EnableHPF_0_5HZ = false;
                PControlForm.EnableHPF_0_05HZ = true;
                if (PControlForm.shimmer.GetSamplingRate() < 50)
                {
                    MessageBox.Show("High Pass Filter only valid for the following frequencies, 51.2Hz, 102.4Hz, 204.8Hz, 256Hz, 512Hz, 1024Hz", 
                        Control.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                PControlForm.EnableHPF_5HZ = false;
                PControlForm.EnableHPF_0_5HZ = false;
                PControlForm.EnableHPF_0_05HZ = false;
            }

            if (checkBoxBSF50.Checked)
            {
                PControlForm.EnableBSF_49_51HZ = true;
                PControlForm.EnableBSF_59_61HZ = false;
                if (PControlForm.shimmer.GetSamplingRate() < 250)
                {
                    MessageBox.Show("Band Stop Filter only valid for the following frequencies, 256Hz, 512Hz, 1024Hz", 
                        Control.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (checkBoxBSF60.Checked)
            {
                PControlForm.EnableBSF_49_51HZ = false;
                PControlForm.EnableBSF_59_61HZ = true;
                if (PControlForm.shimmer.GetSamplingRate() < 250)
                {
                    MessageBox.Show("Band Stop Filter only valid for the following frequencies, 256Hz, 512Hz, 1024Hz", 
                        Control.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                PControlForm.EnableBSF_49_51HZ = false;
                PControlForm.EnableBSF_59_61HZ = false;
            }

            Byte[] config1 = { Convert.ToByte(textBoxChip1Reg1.Text), Convert.ToByte(textBoxChip1Reg2.Text), Convert.ToByte(textBoxChip1Reg3.Text), 
                             Convert.ToByte(textBoxChip1Reg4.Text), Convert.ToByte(textBoxChip1Reg5.Text), Convert.ToByte(textBoxChip1Reg6.Text), 
                             Convert.ToByte(textBoxChip1Reg7.Text), Convert.ToByte(textBoxChip1Reg8.Text), Convert.ToByte(textBoxChip1Reg9.Text), 
                             Convert.ToByte(textBoxChip1Reg10.Text) };
            Byte[] config2 = { Convert.ToByte(textBoxChip2Reg1.Text), Convert.ToByte(textBoxChip2Reg2.Text), Convert.ToByte(textBoxChip2Reg3.Text), 
                             Convert.ToByte(textBoxChip2Reg4.Text), Convert.ToByte(textBoxChip2Reg5.Text), Convert.ToByte(textBoxChip2Reg6.Text), 
                             Convert.ToByte(textBoxChip2Reg7.Text), Convert.ToByte(textBoxChip2Reg8.Text), Convert.ToByte(textBoxChip2Reg9.Text), 
                             Convert.ToByte(textBoxChip2Reg10.Text) };
            PControlForm.shimmer.WriteEXGConfigurations(config1, config2);
        }

    }
}
