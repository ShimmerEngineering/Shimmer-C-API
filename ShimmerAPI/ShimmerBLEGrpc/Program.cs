using com.shimmerresearch.grpc;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerBLEGrpc
{
    class Program
    {
        static void Main(string[] args)
        {

            const int Port = 50052; // Port on which the server will listen

            var server = new Server
            {
                Services = { ShimmerBLEByteServer.BindService(new ShimmerBLEServiceImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine($"Server listening at port {Port}. Press any key to terminate");
            Console.ReadKey();
        }
    }
}
