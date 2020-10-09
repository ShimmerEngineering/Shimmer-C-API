using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.shimmerresearch.grpc;
using Grpc.Core;

namespace com.shimmerresearch.grpc
{
    class ShimmerGrpcImpl 
    {
        ShimmerServer.ShimmerServerClient client;
        public ShimmerGrpcImpl(){


            
        }

        public async Task Start()
        {
            
            Channel channel = new Channel("localhost:50050", ChannelCredentials.Insecure);
            client = new ShimmerServer.ShimmerServerClient(channel);
            var call = client.GetDataStream(new StreamRequest());
            while (await call.ResponseStream.MoveNext())
            {
                var note = call.ResponseStream.Current;
                Console.WriteLine("Received " + note);
            }
            
        }

        public void Connect(string comport)
        {
            var req = new ShimmerRequest();
            req.Address = comport;
            client.ConnectShimmer(req);
        }

        public void Disconnect()
        {
            var req = new ShimmerRequest();
            client.DisconnectShimmer(req);
        }

        public void StartStreaming()
        {
            var req = new ShimmerRequest();
            client.StartStreaming(req);
        }

        public void StopStreaming()
        {
            var req = new ShimmerRequest();
            client.StopStreaming(req);
        }
    }
}
