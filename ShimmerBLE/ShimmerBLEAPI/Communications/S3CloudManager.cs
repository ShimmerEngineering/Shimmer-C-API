
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using shimmer.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBLEAPI.UWP.Communications
{
    /// <summary>
    /// Contains methods related to S3 Cloud server
    /// </summary>
    public class S3CloudManager : ICloudManager
    {
        S3CloudInfo S3CloudInfo;
        /// <summary>
        /// Create a S3CloudManager
        /// </summary>
        /// <param name="s3CloudInfo"></param>
        public S3CloudManager(S3CloudInfo s3CloudInfo)
        {
            S3CloudInfo = s3CloudInfo;
            DeleteAfterUpload = true;
        }

        public event EventHandler<CloudManagerEvent> CloudManagerEvent;
        public TaskCompletionSource<bool> RequestTCS { get; set; }
        public bool DeleteAfterUpload { get; set; }
        /// <summary>
        /// Upload file to the S3 server
        /// </summary>
        /// <param name="filePath">location of the file</param>
        public async Task<bool> UploadFile(object filePath)
        {
            AmazonS3Client s3Client = new AmazonS3Client(new BasicAWSCredentials(S3CloudInfo.S3AccessKey, S3CloudInfo.S3SecretKey), RegionEndpoint.GetBySystemName(S3CloudInfo.S3RegionName));
            TransferUtility transferUtility = new TransferUtility(s3Client);
            CloudManagerEvent += CloudManager_Event;
            RequestTCS = new TaskCompletionSource<bool>();

            try
            {
                bool doesBucketExist = await transferUtility.S3Client.DoesS3BucketExistAsync(S3CloudInfo.S3BucketName);
                if (!doesBucketExist)
                {
                    if (CloudManagerEvent != null)
                        CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadFail, message = "Bucket does not exist." });
                }
                string bucketName = S3CloudInfo.S3BucketName;
                if (!string.IsNullOrEmpty(S3CloudInfo.S3SubFolder))
                {
                    bucketName = S3CloudInfo.S3BucketName + "/" + S3CloudInfo.S3SubFolder;
                }

                TransferUtilityUploadDirectoryRequest uploadRequest = new TransferUtilityUploadDirectoryRequest
                {
                    BucketName = bucketName,
                    Directory = (string)filePath
                };

                uploadRequest.UploadDirectoryProgressEvent += new EventHandler<UploadDirectoryProgressArgs>(uploadRequest_UploadPartProgressEvent);
                await transferUtility.UploadDirectoryAsync(uploadRequest);
                return await RequestTCS.Task;
            }
            catch (Exception ex)
            {
                if (CloudManagerEvent != null)
                    CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadFail, message = ex.Message });
                return false;
            }
            void uploadRequest_UploadPartProgressEvent(object sender, UploadDirectoryProgressArgs e)
            {
                // Process event.
                int progress = (int)((double)e.TransferredBytes / e.TotalBytes * 100.0);
                Console.WriteLine("AWS S3 Data Files Upload Progress {0}%", progress);
                if (progress == 100)
                {
                    if (CloudManagerEvent != null)
                        CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadSuccessful, message = (string)filePath });
                } else
                {
                    if (CloudManagerEvent != null)
                        CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadProgressUpdate, message = progress.ToString() });

                }
            }

            void CloudManager_Event(object sender, CloudManagerEvent e)
            {
                if (e.CurrentEvent == shimmer.Communications.CloudManagerEvent.CloudEvent.UploadSuccessful)
                {
                    Console.WriteLine("File Uploaded: " + e.message);
                    RequestTCS.TrySetResult(true);
                    transferUtility.Dispose();
                    if (DeleteAfterUpload)
                    {
                        DeleteUploadedBinFile(e.message);
                    }
                }
                else if (e.CurrentEvent == shimmer.Communications.CloudManagerEvent.CloudEvent.UploadFail)
                {
                    Console.WriteLine("File Upload Failed: " + e.message);
                    RequestTCS.TrySetResult(false);
                }
            }

        }
        /// <summary>
        /// Delete the uploaded file in the file path
        /// </summary>
        /// <param name="filePath">location of the file</param>
        public bool DeleteUploadedBinFile(object filePath)
        {
            // Delete all files in a directory    
            string[] files = Directory.GetFiles((string)filePath);

            try
            {
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    for (int tries = 0; IsFileLocked(fileInfo) && tries < 5; tries++)
                        Thread.Sleep(200);
                    fileInfo.Delete();
                }
                if (CloudManagerEvent != null)
                    CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadedFileDeleteSuccessful, message = (string)filePath });

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                if (CloudManagerEvent != null)
                    CloudManagerEvent.Invoke(null, new CloudManagerEvent { CurrentEvent = shimmer.Communications.CloudManagerEvent.CloudEvent.UploadedFileDeleteFail, message = e.Message });

                return false;
            }

        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
