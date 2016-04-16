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
using com.shimmerresearch.datastructure;


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
            MemoryStream memorys = new MemoryStream();
            Queue<byte> byteQ = new Queue<byte>();
            string ip = "127.0.0.1";
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 5000;
            Socket newClient;
            TcpListener serverSocket = new TcpListener(ipAddress,port);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");
            
            clientSocket = serverSocket.AcceptTcpClient();
            int count = 0;
            int state = 0;
            int packetSize = 4;
            while (true)
            {
                
                
                NetworkStream networkStream = clientSocket.GetStream();
                byte[] arrayB = new byte[packetSize];
                int length = networkStream.Read(arrayB,count,packetSize-count);
                memorys.Write(arrayB,count,length);
                count = count + length;
                if (state == 0)
                {
                    if (memorys.Length >= packetSize)
                    {
                        arrayB = new byte[4];
                        memorys.Position = 0;
                        memorys.Read(arrayB, 0, 4);
                        Array.Reverse(arrayB);
                        packetSize = BitConverter.ToInt32(arrayB,0);
                        state = 1;
                        count = 0;
                        memorys.SetLength(0);
                    }
                }
                else
                {
                    if (memorys.Length >= packetSize)
                    {
                        arrayB = new byte[packetSize];
                        memorys.Position = 0;
                        memorys.Read(arrayB, 0, packetSize);
                        //Array.Reverse(arrayB);
                        ObjectCluster ojc = ObjectCluster.Parser.ParseFrom(arrayB);
                        Console.WriteLine(ojc.Name);
                        state = 0;
                        packetSize = 4;
                        count = 0;
                        memorys.SetLength(0);
                    }
                }
               
                
                
            }

        }
    }
}
