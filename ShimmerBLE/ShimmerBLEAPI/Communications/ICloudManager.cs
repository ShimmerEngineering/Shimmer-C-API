using ShimmerBLEAPI.Models;
using System;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public interface ICloudManager
    {
        event EventHandler<CloudManagerEvent> CloudManagerEvent;
        Task<bool> UploadFile(Object file);
        bool DeleteUploadedBinFile(Object file);
    }

    /// <summary>
    /// Store the event data
    /// </summary>
    public class CloudManagerEvent
    {
        public enum CloudEvent
        {
            UploadSuccessful = 1,
            UploadFail = 2,
            UploadProgressUpdate = 3,
            UploadedFileDeleteSuccessful = 4,
            UploadedFileDeleteFail = 5
        }
        public CloudEvent CurrentEvent;
        public string message;
        public Object objMsg;
    }
}
