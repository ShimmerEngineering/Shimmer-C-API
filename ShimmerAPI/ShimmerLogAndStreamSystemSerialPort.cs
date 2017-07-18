namespace ShimmerAPI
{

    public class ShimmerLogAndStreamSystemSerialPort : ShimmerLogAndStream
    {
        protected override int ReadByte()
        {
            return SerialPort.ReadByte();
        }
        protected override void OpenConnection()
        {
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = ComPort;
            SerialPort.ReadTimeout = this.ReadTimeout;
            SerialPort.WriteTimeout = this.WriteTimeout;
            SetState(SHIMMER_STATE_CONNECTING);
            try
            {
                SerialPort.Open();
            }
            catch
            {
            }
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }
    }
}
