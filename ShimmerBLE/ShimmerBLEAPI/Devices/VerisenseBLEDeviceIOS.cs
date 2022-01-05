using shimmer.Communications;
using shimmer.Models;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBLEAPI.Devices
{ 
    public class VerisenseBLEDeviceIOS : VerisenseBLEDevice
    {
        public VerisenseBLEDeviceIOS(string id, string name) : base(id, name)
        {
        }

        public async Task<bool> PairDeviceAsync()
        {
            var result = await Connect(false);
           
            /*
            InitializeRadio();
            BLERadio.Asm_uuid = Asm_uuid;
            BLERadio.CommunicationEvent += UartRX_ValueUpdated_ForPairing;
            var result = await BLERadio.Connect();
            await BLERadio.WriteBytes(ReadProdConfigRequest);
        */

            for (int i = 0; i < 10; i++)
            {
                ProdConfigPayload prodConfig = (ProdConfigPayload)await ExecuteRequest(RequestType.ReadProductionConfig);
                if (prodConfig != null)
                {
                    await Disconnect();
                    return true;
                } else
                {
                    Debug.WriteLine("Pairing Failed: " +  i);
                }
                Thread.Sleep(100);
            }
            await Disconnect();
            return false;
        }
    }
}
