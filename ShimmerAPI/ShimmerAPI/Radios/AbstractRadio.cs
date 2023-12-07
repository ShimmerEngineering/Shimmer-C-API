using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Radios
{
    public abstract class AbstractRadio
    {
        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract bool WriteBytes(byte[] bytes);
    }
}
