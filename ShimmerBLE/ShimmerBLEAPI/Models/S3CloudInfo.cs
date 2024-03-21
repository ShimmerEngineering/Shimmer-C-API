using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Models
{
    /// <summary>
    /// This class is used to store S3 cloud information
    /// </summary>
    public class S3CloudInfo
    {
        public string S3AccessKey { get; set; }
        public string S3SecretKey { get; set; }
        public string S3BucketName { get; set; }
        public string S3RegionName { get; set; }
        public string S3SubFolder { get; set; }
    }
}
