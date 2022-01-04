using shimmer.Models;
using shimmer.Sensors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public interface IVerisenseBLE
    {
        /// <summary>
        /// Event Handler for state changes, new data packets, and updates when transferring logged data
        /// </summary>
        event EventHandler<ShimmerBLEEventData> ShimmerBLEEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialize">Setting to true, will cause the class to automatically retrieve the operational config, production config, and status</param>
        /// <returns></returns>
        Task<bool> Connect(bool initialize);
        Task<bool> Disconnect();

        /// <summary>
        /// e.g. ExecuteRequest(RequestType.ReadStatus) or ExecuteRequest(RequestType.WriteRTC) or ExecuteRequest(RequestType.WriteOperationalConfig, opConfigBytesArray). Where opConfigBytesArray is byte[]. Note we recommend using WriteAndReadOperationalConfiguration when writing operational config bytes because it automatically does the read back. E.g. If some dependencies arent met, the operational configurations bytes read might be different from that which was written.
        /// </summary>
        /// <param name="reqObjects">First param is the RequestType and second parameter IF required is the byte[]</param>
        /// <returns></returns>
        Task<IBasePayload> ExecuteRequest(params Object[] reqObjects);
        /// <summary>
        /// Does both ExecuteRequest(RequestType.WriteOperationalConfig) and ExecuteRequest(RequestType.ReadOperationalConfig), so you can double check the operational configuration bytes which have been accepted by the Verisense device. E.g. if some dependencies arent met, the operational configurations bytes read might be different.
        /// </summary>
        /// <param name="operationalConfiguration"></param>
        /// <returns></returns>
        Task<IBasePayload> WriteAndReadOperationalConfiguration(byte[] operationalConfiguration);
        ShimmerDeviceBluetoothState GetVerisenseBLEState();
    }
    public enum ShimmerDeviceBluetoothState
    {
        None,
        Disconnected,
        Connecting,
        Connected,
        Streaming,
        StreamingLoggedData,
        Limited,
        NotPairedAndDisconnected
    }
}
