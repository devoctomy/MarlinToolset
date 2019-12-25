using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Services
{
    public interface ISerialPortAdapter
    {
        SerialPortAdapterRef Connect(
            string port,
            int baudRate,
            Action<SerialPortAdapterRef, string> dataReceivedCallback);

        void Disconnect(SerialPortAdapterRef portRef);

        void Write(
            SerialPortAdapterRef portRef,
            string data,
            Encoding encoding);

        void Write(
            SerialPortAdapterRef portRef,
            byte[] data,
            int offset,
            int count);
    }
}
