using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Services
{
    public class SerialPortAdapterRef
    {
        public string Port { get; }
        public int BaudRate { get; }
        public Action<SerialPortAdapterRef, string> DataReceivedCallback { get; }

        public SerialPortAdapterRef(
            string port,
            int baudRate,
            Action<SerialPortAdapterRef, string> dataReceivedCalllback)
        {
            Port = port;
            BaudRate = baudRate;
            DataReceivedCallback = dataReceivedCalllback;
        }
    }
}
