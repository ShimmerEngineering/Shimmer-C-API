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

namespace SpeedTestExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SerialPortRadio radio;
        SpeedTestProtocol SerialPortSpeedTestProtocol;
        SpeedTestProtocol BLE32FeetSpeedTestProtocol;
        SpeedTestProtocol TestRadioSpeedTestProtocol;
        BLE32FeetRadio radioBLE;
        TestRadio testRadio;
        private void button1_Click(object sender, EventArgs e)
        {
            radio = new SerialPortRadio(textBox2.Text);
            SerialPortSpeedTestProtocol = new SpeedTestProtocol(radio);
            SerialPortSpeedTestProtocol.Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialPortSpeedTestProtocol.StartTestSignal();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            radioBLE = new BLE32FeetRadio(textBox1.Text, BLE32FeetRadio.DeviceType.Shimmer3BLE);
            BLE32FeetSpeedTestProtocol = new SpeedTestProtocol(radioBLE);
            BLE32FeetSpeedTestProtocol.Connect();
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
            testRadio = new TestRadio();
            TestRadioSpeedTestProtocol = new SpeedTestProtocol(testRadio);
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
    }
}
