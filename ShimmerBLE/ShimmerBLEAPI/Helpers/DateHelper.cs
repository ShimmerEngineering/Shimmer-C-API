using Flurl;
using Flurl.Http;
using Flurl.Http.Xml;
using shimmer.DTO;
using System;
using System.Threading.Tasks;

namespace shimmer.Helpers
{
    /// <summary>
    /// This class contains date related methods
    /// </summary>
    public static class DateHelper
    {
        public static DateTime Start = new DateTime(1970, 1, 1);
        public static DateTime StartUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts date to unix timestamp in milliseconds
        /// </summary>
        /// <param name="date">date to be convert</param>
        /// <returns>unix timestamp in milliseconds</returns>
        public static long GetTimestamp(DateTime date)
        {
            var diff = (date - Start).TotalMilliseconds;
            return (long)diff;
        }

        /// <summary>
        /// Returns the total milliseconds in a time span
        /// </summary>
        /// <param name="span"></param>
        /// <returns>total milliseconds</returns>
        public static long GetTimestamp(TimeSpan span)
        {
            return (long)span.TotalMilliseconds;
        }

        /// <summary>
        /// Convert unix timestamp in milliseconds into DateTime
        /// </summary>
        /// <param name="timestamp">unix timestamp in milliseconds</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTime(long timestamp)
        {
            var date = Start.AddMilliseconds(timestamp);
            return date;
        }

        /// <summary>
        /// Convert unix timestamp in seconds into DateTime
        /// </summary>
        /// <param name="timestamp">unix timestamp in seconds</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTimeFromSeconds(double timestamp)
        {
            var date = Start.AddSeconds(timestamp);
            return date;
        }

        /// <summary>
        /// Convert unix timestamp in milliseconds into DateTime in which the DateTimeKind is UTC
        /// </summary>
        /// <param name="timestamp">unix timestamp in milliseconds</param>
        /// <returns>DateTime with DateTimeKind UTC</returns>
        public static DateTime GetDateTimeFromUnixTimestampMillis(double timestamp)
        {
            var date = StartUtc.AddMilliseconds(timestamp);
            return date;
        }

        /// <summary>
        /// Convert timestamp in milliseconds into TimeSpan
        /// </summary>
        /// <param name="timestamp">timestamp in milliseconds</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan GetTimeSpan(long timestamp)
        {
            return TimeSpan.FromMilliseconds(timestamp);
        }

        /// <summary>
        /// Returns current unix timestamp in milliseconds
        /// </summary>
        /// <returns>current unix timestamp in milliseconds</returns>
        public static double GetUnixTimestampMillis()
        {
            return (double)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        #region Internet Time

        /// <summary>
        /// Returns nist time
        /// </summary>
        /// <returns></returns>
        public static async Task<long> GetNistTime()
        {
            try
            {
                var url = new Url("http://nist.time.gov");

                var data = await url
                    .AppendPathSegment("actualtime.cgi")
                    .SetQueryParam("lzbc", "siqm9b")
                    .AllowAnyHttpStatus()
                    .GetXmlAsync<NistTime>();

                var ts = long.Parse(data.Time.Substring(0, 13));

                return ts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        #endregion

    }
}
