using Flurl;
using Flurl.Http;
using Flurl.Http.Xml;
using shimmer.DTO;
using System;
using System.Threading.Tasks;

namespace shimmer.Helpers
{
    public static class DateHelper
    {
        public static DateTime Start = new DateTime(1970, 1, 1);
        public static DateTime StartUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long GetTimestamp(DateTime date)
        {
            var diff = (date - Start).TotalMilliseconds;
            return (long)diff;
        }

        public static long GetTimestamp(TimeSpan span)
        {
            return (long)span.TotalMilliseconds;
        }

        public static DateTime GetDateTime(long timestamp)
        {
            var date = Start.AddMilliseconds(timestamp);
            return date;
        }

        public static DateTime GetDateTimeFromSeconds(double timestamp)
        {
            var date = Start.AddSeconds(timestamp);
            return date;
        }

        public static DateTime GetDateTimeFromUnixTimestampMillis(double timestamp)
        {
            var date = StartUtc.AddMilliseconds(timestamp);
            return date;
        }

        public static TimeSpan GetTimeSpan(long timestamp)
        {
            return TimeSpan.FromMilliseconds(timestamp);
        }

        public static double GetUnixTimestampMillis()
        {
            return (double)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        #region Internet Time

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
