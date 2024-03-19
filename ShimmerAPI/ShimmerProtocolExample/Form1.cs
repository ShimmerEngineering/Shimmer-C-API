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

namespace ShimmerProtocolExample
{
    public partial class Form1 : Form
    {
        ShimmerByteProtocol protocol;
        AbstractRadio radio;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String comport = textBox1.Text;
            radio = new SerialPortRadio(textBox1.Text);
            protocol = new ShimmerByteProtocol(radio);
            protocol.Connect();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            protocol.Disconnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            protocol.Inquiry();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            protocol.StartStreaming();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            protocol.StopStreaming();
        }
    }
}
