using Shimmer32FeetAPI.Radios;
using ShimmerAPI.Protocols;
using ShimmerAPI.Radios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SpeedTestExample
{
    public partial class Form1 : Form
    {
        private static bool canUpdate = true;
        public Form1()
        {
            InitializeComponent();

        }
        System.Threading.Timer timer = new System.Threading.Timer(EnableUpdate, null, 0, 1000); // Enable update every second
        private static void EnableUpdate(object state)
        {
            canUpdate = true; // Enable update every second
        }

        SerialPortRadio radio;
        SpeedTestProtocol SerialPortSpeedTestProtocol;
        SpeedTestProtocol BLE32FeetSpeedTestProtocol;
        SpeedTestProtocol TestRadioSpeedTestProtocol;
        BLE32FeetRadio radioBLE;
        TestRadio testRadio;
        private void button1_Click(object sender, EventArgs e)
        {
            if (radio != null)
            {
                radio.RadioStatusChanged -= RadioStateChanged;
            }
            radio = new SerialPortRadio(textBox2.Text);
            radio.RadioStatusChanged += RadioStateChanged;
            if (SerialPortSpeedTestProtocol != null)
            {
                SerialPortSpeedTestProtocol.ResultUpdate -= ResultUpdated;
            }
            SerialPortSpeedTestProtocol = new SpeedTestProtocol(radio);
            SerialPortSpeedTestProtocol.ResultUpdate += ResultUpdated;
            SerialPortSpeedTestProtocol.Connect();
        }

        private void ResultUpdated(object sender, string e)
        {
            if (canUpdate)
            {
                SetTextResult(e);
                canUpdate = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialPortSpeedTestProtocol.StartTestSignal();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioBLE != null)
            {
                radioBLE.RadioStatusChanged -= RadioStateChanged;
            }
            radioBLE = new BLE32FeetRadio(textBox1.Text, BLE32FeetRadio.DeviceType.Shimmer3BLE);
            radioBLE.RadioStatusChanged += RadioStateChanged;
            if (BLE32FeetSpeedTestProtocol != null)
            {
                BLE32FeetSpeedTestProtocol.ResultUpdate -= ResultUpdated;
            }
            BLE32FeetSpeedTestProtocol = new SpeedTestProtocol(radioBLE);
            BLE32FeetSpeedTestProtocol.ResultUpdate += ResultUpdated;
            BLE32FeetSpeedTestProtocol.Connect();
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label5.Text = text;
            }
        }


        delegate void SetTextResultCallback(string text);

        private void SetTextResult(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label5.InvokeRequired)
            {
                SetTextResultCallback d = new SetTextResultCallback(SetTextResult);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label8.Text = text;
            }
        }



        private void RadioStateChanged(object sender, AbstractRadio.RadioStatus e)
        {
            SetText(e.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.StartTestSignal();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SerialPortSpeedTestProtocol.StopTestSignal();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            SerialPortSpeedTestProtocol.Disconnect();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.StopTestSignal();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.Disconnect();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (testRadio != null)
            {
                testRadio.RadioStatusChanged -= RadioStateChanged;
            }
            testRadio = new TestRadio();
            testRadio.RadioStatusChanged += RadioStateChanged;
            if (TestRadioSpeedTestProtocol != null)
            {
                TestRadioSpeedTestProtocol.ResultUpdate -= ResultUpdated;
            }
            TestRadioSpeedTestProtocol = new SpeedTestProtocol(testRadio);
            TestRadioSpeedTestProtocol.ResultUpdate += ResultUpdated;
            TestRadioSpeedTestProtocol.Connect();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TestRadioSpeedTestProtocol.StartTestSignal();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            TestRadioSpeedTestProtocol.StopTestSignal();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (radioBLE != null)
            {
                radioBLE.RadioStatusChanged -= RadioStateChanged;
            }
            radioBLE = new BLE32FeetRadio(textBox1.Text, BLE32FeetRadio.DeviceType.Shimmer3R);
            radioBLE.RadioStatusChanged += RadioStateChanged;
            if (BLE32FeetSpeedTestProtocol != null)
            {
                BLE32FeetSpeedTestProtocol.ResultUpdate -= ResultUpdated;
            }
            BLE32FeetSpeedTestProtocol = new SpeedTestProtocol(radioBLE);
            BLE32FeetSpeedTestProtocol.ResultUpdate += ResultUpdated;
            BLE32FeetSpeedTestProtocol.Connect();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.StartTestSignal();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.StopTestSignal();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            BLE32FeetSpeedTestProtocol.Disconnect();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            testRadio.Disconnect();
        }
    }
}
