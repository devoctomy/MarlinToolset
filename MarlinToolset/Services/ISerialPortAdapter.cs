using MarlinToolset.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Services
{
    public interface ISerialPortAdapter
    {
        IReadOnlyList<SerialPortAdapterRef> PortRefs { get; }

        SerialPortAdapterRef Connect(
            PrinterConfigurationModel config,
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

        ISerialPort GetSerialPort(SerialPortAdapterRef portRef);
    }
}
