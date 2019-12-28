using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public interface ISerialPort : IDisposable
    {
        event SerialDataReceivedEventHandler DataReceived;
        bool IsOpen { get; }
        string PortName { get; }
        int BaudRate { get; }
        void Open();
        void Close();
        void Write(byte[] data, int offset, int count);
        string ReadExisting();
    }
}
