using Shimmer32FeetAPI.Radios;
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
        BLE32FeetRadio radioBLE;
        private void button1_Click(object sender, EventArgs e)
        {
            radio = new SerialPortRadio(textBox2.Text);
            radio.Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radio.StartTestSignal();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            radioBLE = new BLE32FeetRadio(textBox1.Text);
            radioBLE.Connect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            radioBLE.StartTestSignal();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
