using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Services
{
    public interface ISerialPort : IDisposable
    {
        event SerialDataReceivedEventHandler DataReceived;
        bool IsOpen { get; }
        void Open();
        void Close();
        void Write(byte[] data, int offset, int count);
        string ReadExisting();
    }
}
