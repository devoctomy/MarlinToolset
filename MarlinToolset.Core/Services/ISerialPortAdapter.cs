using MarlinToolset.Domain.Model;
using System;
using System.Collections.Generic;

namespace MarlinToolset.Core.Services
{
    public interface ISerialPortAdapter
    {
        IReadOnlyList<SerialPortAdapterRef> PortRefs { get; }

        SerialPortAdapterRef Connect(
            PrinterConfigurationModel config,
            Action<SerialPortAdapterRef, string> dataReceivedCallback);

        bool Disconnect(SerialPortAdapterRef portRef);

        bool Write(
            SerialPortAdapterRef portRef,
            byte[] data,
            int offset,
            int count);

        ISerialPort GetSerialPort(SerialPortAdapterRef portRef);
    }
}
