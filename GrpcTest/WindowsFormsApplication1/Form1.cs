using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using Grpc.Core;
using Helloworld;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            var client = Greeter.NewClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "Lim Jong Chern" });
            Console.WriteLine("Greeting: " + reply.Message);
        }
    }
}
